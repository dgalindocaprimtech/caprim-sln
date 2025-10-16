using StellarDotnetSdk;
using StellarDotnetSdk.Assets;
using StellarDotnetSdk.Memos;
using StellarDotnetSdk.Responses;
using StellarDotnetSdk.Responses.Operations;
using StellarDotnetSdk.Xdr;
using System;
using System.Threading.Tasks;

public class StellarListener
{
    private const string AccountIdToWatch = "GCVJKYKGYWKODWXKONOQQTHZG34QBZCMEYN4PO5N6V2FCF4DXY6RXPX2";
    private const string HorizonUrl = "https://horizon.stellar.org";
    private static Server _server;

    public static async Task Main(string[] args)
    {
        _server = new Server(HorizonUrl);
        Console.WriteLine($"Listening for new payments for account: {AccountIdToWatch}...");
        try
        {
            var listener = _server.Transactions
                .ForAccount(AccountIdToWatch)
                .Stream(OnMessageTransactions)
                .Connect();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            // listener.Shutdown();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void OnMessageOperations(object sender, OperationResponse response)
    {
        Console.WriteLine("\n--- Account activated with initial deposit! ---");
    }

    private static async void OnMessageTransactions(object sender, TransactionResponse response)
    {
        try
        {
            Console.WriteLine("\n--- Account activated with initial deposit! ---");
            if (response is StellarDotnetSdk.Responses.TransactionResponse)
            {
                switch (response.Memo)
                {

                    case MemoText _:
                        Console.WriteLine($"Memo Text: {response.MemoValue}");
                        break;
                    case MemoId _:
                        Console.WriteLine($"Memo ID: {response.MemoValue}");
                        break;
                    case MemoHash _:
                        Console.WriteLine($"Memo Hash: {response.MemoValue}");
                        break;
                    case MemoReturnHash _:
                        Console.WriteLine($"Memo Return Hash: {response.MemoValue}");
                        break;
                    default:
                        Console.WriteLine("Unknown Memo");
                        break;
                }
                var operation = await _server.Operations.ForTransaction(response.Hash).Execute();
                if (operation.Records.Count > 0 && operation.Records[0] is PaymentOperationResponse payment)
                {
                    Console.WriteLine("\n--- New Payment Received! ---");
                    Console.WriteLine($"  - From: {payment.From}");
                    Console.WriteLine($"  - Amount: {payment.Amount} {payment.AssetType}");
                    Console.WriteLine($"  - Transaction ID: {payment.TransactionHash}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing transaction: {ex.Message}");
        }
    }

    // Example for future extensibility: handle payment operations
    private static void OnMessage(object sender, OperationResponse response)
    {
        if (response is PaymentOperationResponse payment)
        {
            if (payment.To == AccountIdToWatch)
            {
                string assetName = payment.Asset switch
                {
                    AssetTypeNative _ => "XLM",
                    AssetTypeCreditAlphaNum4 asset4 => $"{asset4.Code}:{asset4.Issuer}",
                    AssetTypeCreditAlphaNum12 asset12 => $"{asset12.Code}:{asset12.Issuer}",
                    _ => "Unknown Asset"
                };
                Console.WriteLine($"--- Payment Received ---\nFrom: {payment.From}\nAmount: {payment.Amount} {assetName}\nTransaction ID: {payment.TransactionHash}\n---------------------------\n");
            }
        }
        else if (response is CreateAccountOperationResponse createAccount)
        {
            if (createAccount.Account == AccountIdToWatch)
            {
                Console.WriteLine("\n--- Account activated with initial deposit! ---");
                Console.WriteLine($"  - Funder: {createAccount.Funder}");
                Console.WriteLine($"  - Starting Balance: {createAccount.StartingBalance} XLM");
                Console.WriteLine($"  - Transaction ID: {createAccount.TransactionHash}");
                Console.WriteLine("-------------------------------------------------\n");
            }
        }
    }
}