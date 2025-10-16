-- Database Schema Creation Script for Stellar Custody Platform
-- Version: 1.0
-- Date: 2025-09-17
-- Description: Creates all tables, constraints, and indexes based on the proposed architecture.

-- Enable UUID extension if not already enabled
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Table: user_statuses (Catalog table for user statuses)
CREATE TABLE user_statuses (
    id INTEGER PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: kyc_levels (Catalog table for KYC levels, with country support)
CREATE TABLE kyc_levels (
    id INTEGER PRIMARY KEY,
    level_name VARCHAR(50) NOT NULL,
    country_code CHAR(2) NOT NULL, -- ISO 3166-1 alpha-2
    description VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(level_name, country_code)
);

-- Table: document_types (Catalog table for document types, with country support)
CREATE TABLE document_types (
    id INTEGER PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    country_code CHAR(2) NOT NULL, -- ISO 3166-1 alpha-2
    description VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(name, country_code)
);

-- Table: banks (Catalog table for banks, with country support)
CREATE TABLE banks (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    country_code CHAR(2) NOT NULL, -- ISO 3166-1 alpha-2
    code VARCHAR(10), -- Bank code if applicable
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(name, country_code)
);

-- Table: bank_account_types (Catalog table for bank account types, with country support)
CREATE TABLE bank_account_types (
    id INTEGER PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    country_code CHAR(2) NOT NULL, -- ISO 3166-1 alpha-2
    description VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(name, country_code)
);

-- Table: assets (Catalog table for Stellar assets)
CREATE TABLE assets (
    id INTEGER PRIMARY KEY,
    code VARCHAR(12) NOT NULL UNIQUE, -- Asset code (e.g., 'USDC', 'XLM')
    issuer VARCHAR(56), -- Public key of issuer, NULL for native XLM
    name VARCHAR(100),
    type VARCHAR(20) NOT NULL, -- 'native' or 'credit_alphanum4' or 'credit_alphanum12'
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: users
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cognito_sub VARCHAR(255) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    user_status_id INTEGER NOT NULL REFERENCES user_statuses(id) ON DELETE CASCADE ON UPDATE CASCADE,
    kyc_level_id INTEGER NOT NULL REFERENCES kyc_levels(id) ON DELETE CASCADE ON UPDATE CASCADE,
    kyc_date DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: user_profiles
CREATE TABLE user_profiles (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE,
    encrypted_first_name VARCHAR(500),
    encrypted_last_name VARCHAR(500),
    encrypted_address VARCHAR(1000),
    encrypted_phone VARCHAR(500),
    encrypted_document_number VARCHAR(500),
    document_type_id INTEGER REFERENCES document_types(id) ON DELETE CASCADE ON UPDATE CASCADE,
    nationality VARCHAR(100),
    city VARCHAR(100),
    gender VARCHAR(10),
    birth_date DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(user_id)
);

-- Table: stellar_accounts
CREATE TABLE stellar_accounts (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE,
    public_key VARCHAR(56) UNIQUE NOT NULL, -- Stellar public key
    kms_key_arn VARCHAR(255) UNIQUE NOT NULL, -- ARN of the KMS key
    account_name VARCHAR(100),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: bank_accounts
CREATE TABLE bank_accounts (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE ON UPDATE CASCADE,
    bank_id INTEGER NOT NULL REFERENCES banks(id) ON DELETE CASCADE ON UPDATE CASCADE,
    account_type_id INTEGER NOT NULL REFERENCES bank_account_types(id) ON DELETE CASCADE ON UPDATE CASCADE,
    encrypted_account_number VARCHAR(500),
    holder_name VARCHAR(255),
    holder_document_id VARCHAR(255),
    holder_document_type_id INTEGER REFERENCES document_types(id) ON DELETE CASCADE ON UPDATE CASCADE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: transactions
CREATE TABLE transactions (
    id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    stellar_account_id UUID NOT NULL REFERENCES stellar_accounts(id) ON DELETE CASCADE ON UPDATE CASCADE,
    stellar_tx_hash VARCHAR(64) UNIQUE NOT NULL, -- Stellar transaction hash
    asset_id INTEGER NOT NULL REFERENCES assets(id) ON DELETE CASCADE ON UPDATE CASCADE,
    type VARCHAR(50) NOT NULL, -- e.g., 'payment', 'trustline'
    amount DECIMAL(18,8) NOT NULL,
    processed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: exchange_rates
CREATE TABLE exchange_rates (
    id INTEGER PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    base_asset_id INTEGER NOT NULL REFERENCES assets(id) ON DELETE CASCADE ON UPDATE CASCADE,
    quote_asset_id INTEGER NOT NULL REFERENCES assets(id) ON DELETE CASCADE ON UPDATE CASCADE,
    rate DECIMAL(18,8) NOT NULL,
    provider VARCHAR(100),
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(base_asset_id, quote_asset_id, timestamp)
);

-- Table: exchange_rates_history
CREATE TABLE exchange_rates_history (
    id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    exchange_rate_id INTEGER NOT NULL REFERENCES exchange_rates(id) ON DELETE CASCADE ON UPDATE CASCADE,
    old_rate DECIMAL(18,8),
    new_rate DECIMAL(18,8) NOT NULL,
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Indexes for performance
CREATE INDEX idx_users_cognito_sub ON users(cognito_sub);
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_user_status_id ON users(user_status_id);
CREATE INDEX idx_users_kyc_level_id ON users(kyc_level_id);

CREATE INDEX idx_user_profiles_user_id ON user_profiles(user_id);
CREATE INDEX idx_user_profiles_document_type_id ON user_profiles(document_type_id);

CREATE INDEX idx_stellar_accounts_user_id ON stellar_accounts(user_id);
CREATE INDEX idx_stellar_accounts_public_key ON stellar_accounts(public_key);
CREATE INDEX idx_stellar_accounts_kms_key_arn ON stellar_accounts(kms_key_arn);

CREATE INDEX idx_bank_accounts_user_id ON bank_accounts(user_id);
CREATE INDEX idx_bank_accounts_bank_id ON bank_accounts(bank_id);
CREATE INDEX idx_bank_accounts_account_type_id ON bank_accounts(account_type_id);
CREATE INDEX idx_bank_accounts_holder_document_type_id ON bank_accounts(holder_document_type_id);

CREATE INDEX idx_transactions_stellar_account_id ON transactions(stellar_account_id);
CREATE INDEX idx_transactions_asset_id ON transactions(asset_id);
CREATE INDEX idx_transactions_stellar_tx_hash ON transactions(stellar_tx_hash);
CREATE INDEX idx_transactions_processed_at ON transactions(processed_at);

CREATE INDEX idx_exchange_rates_base_asset_id ON exchange_rates(base_asset_id);
CREATE INDEX idx_exchange_rates_quote_asset_id ON exchange_rates(quote_asset_id);
CREATE INDEX idx_exchange_rates_timestamp ON exchange_rates(timestamp);

CREATE INDEX idx_exchange_rates_history_exchange_rate_id ON exchange_rates_history(exchange_rate_id);
CREATE INDEX idx_exchange_rates_history_changed_at ON exchange_rates_history(changed_at);

-- Comments
COMMENT ON TABLE users IS 'Stores user information linked to Cognito';
COMMENT ON TABLE user_profiles IS 'Encrypted personal information of users';
COMMENT ON TABLE stellar_accounts IS 'Stellar accounts managed via KMS';
COMMENT ON TABLE transactions IS 'Records of Stellar transactions';
COMMENT ON TABLE exchange_rates IS 'Current exchange rates between assets';
COMMENT ON TABLE exchange_rates_history IS 'Historical changes in exchange rates';