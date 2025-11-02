#!/bin/bash
# Wait for SQL Server to start
sleep 30

# Run the SQL script
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U SA -P mssqlP@ss -d master -i /usr/src/app/init/UserVaultDBSchema.sql