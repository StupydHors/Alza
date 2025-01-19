param (
    [switch]$WithSeedData
)

Write-Host "Resetting database..." -ForegroundColor Yellow

# Set environment variable for connection string
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=ProjectNameDb;User Id=sa;Password=Your_Strong_Password123;TrustServerCertificate=True;"

# Remove existing migrations
Write-Host "Removing existing database..." -ForegroundColor Yellow
dotnet ef database drop -f -p src/ProjectName.Infrastructure -s src/ProjectName.Api

# Remove migrations folder
Write-Host "Removing migrations..." -ForegroundColor Yellow
Remove-Item -Path "src/ProjectName.Infrastructure/Persistence/Migrations" -Recurse -ErrorAction SilentlyContinue

# Add new migration
Write-Host "Creating new migration..." -ForegroundColor Yellow
dotnet ef migrations add InitialCreate -p src/ProjectName.Infrastructure -s src/ProjectName.Api

# Update database
Write-Host "Applying migration..." -ForegroundColor Yellow
dotnet ef database update -p src/ProjectName.Infrastructure -s src/ProjectName.Api

if ($WithSeedData) {
    Write-Host "Seeding database..." -ForegroundColor Yellow
    dotnet run --project src/ProjectName.Api -- --seed
}

Write-Host "Database reset completed!" -ForegroundColor Green