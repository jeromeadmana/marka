-- Test Data for Marka Application
-- This script creates a test customer, test user, and sample markas around Manila

-- Insert test customer
INSERT INTO "Customers" ("Id", "Name", "CreatedAt", "UpdatedAt")
VALUES
    ('a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'Test Company', NOW(), NOW())
ON CONFLICT ("Id") DO NOTHING;

-- Insert test user
INSERT INTO "Users" ("Id", "Name", "Email", "CustomerId", "CreatedAt", "UpdatedAt")
VALUES
    ('b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', 'Test User', 'testuser@marka.com', 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', NOW(), NOW())
ON CONFLICT ("Id") DO NOTHING;

-- Insert sample markas around Manila
INSERT INTO "markas" ("Id", "Name", "Description", "Latitude", "Longitude", "Address", "Category", "Status", "CustomerId", "CreatedByUserId", "CreatedAt", "UpdatedAt")
VALUES
    -- Intramuros
    ('c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f', 'Fort Santiago', 'Historic walled fortress', 14.5929, 120.9738, 'Intramuros, Manila', 'Historical', 'Active', 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', NOW(), NOW()),

    -- Rizal Park
    ('d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a', 'Rizal Monument', 'National hero monument', 14.5833, 120.9789, 'Rizal Park, Manila', 'Historical', 'Active', 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', NOW(), NOW()),

    -- SM Mall of Asia
    ('e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b', 'SM Mall of Asia', 'Large shopping mall', 14.5352, 120.9823, 'Pasay, Metro Manila', 'Shopping', 'Active', 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', NOW(), NOW()),

    -- Quezon Memorial Circle
    ('f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c', 'Quezon Memorial', 'Memorial shrine and park', 14.6527, 121.0499, 'Quezon City', 'Park', 'Active', 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', NOW(), NOW()),

    -- Bonifacio Global City
    ('a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d', 'BGC High Street', 'Modern urban district', 14.5514, 121.0471, 'Taguig, Metro Manila', 'Commercial', 'Active', 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', NOW(), NOW())
ON CONFLICT ("Id") DO NOTHING;

-- Verify the data
SELECT 'Customers' as table_name, COUNT(*) as count FROM "Customers"
UNION ALL
SELECT 'Users', COUNT(*) FROM "Users"
UNION ALL
SELECT 'markas', COUNT(*) FROM "markas";
