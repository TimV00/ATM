#!/bin/bash
set -e

echo "=== Restoring dependencies ==="
dotnet restore

echo "=== Building (Debug) ==="
dotnet build --configuration Debug --no-restore

echo "=== Building (Release) ==="
dotnet build --configuration Release --no-restore

echo "=== Running tests with coverage ==="
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Exclude="[ATM]dal.*,[ATM]service.*,[ATM]util.*,[ATM]ATM.Program"

echo "=== Generating documentation ==="
doxygen Doxyfile

echo "=== Building PDF ==="
cd docs/latex && make
cd ../..

echo "=== Build complete ==="
