# Adres API
$baseUrl = "https://localhost:7130/api/users"

function Wait-Continue($message) {
    Write-Host ""
    Read-Host -Prompt "$message (naciœnij Enter, aby kontynuowaæ)"
}

# 1. GET - Pobranie wszystkich u¿ytkowników
Write-Host "1. GET /api/users"
$response = Invoke-RestMethod -Uri $baseUrl -Method Get
Write-Host ($response | ConvertTo-Json -Depth 5)
Wait-Continue "Zakoñczono GET wszystkich u¿ytkowników"

# 2. POST - Dodanie nowego u¿ytkownika
Write-Host "`n2. POST /api/users"

$newUserJson = @"
{
    "id": 0,
    "firstname": "Tomasz",
    "lastname": "Kowalski",
    "dateOfBirth": "1990-05-15T00:00:00",
    "sex": "Male",
    "customProperties": [
        { "id": 0, "name": "Hobby", "Value": "Pi³ka no¿na" },
        { "id": 0, "Name": "Miasto", "Value": "Warszawa" }
    ]
}
"@

$response = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $newUserJson -ContentType "application/json"
$newUserId = $response.Id
Write-Host "Utworzono u¿ytkownika o ID: $newUserId"
Write-Host ($response | ConvertTo-Json -Depth 5)
Wait-Continue "Zakoñczono POST (utworzenie u¿ytkownika)"

# 3. GET - Pobranie dodanego u¿ytkownika
Write-Host "`n3. GET /api/users/$newUserId"
$response = Invoke-RestMethod -Uri "$baseUrl/$newUserId" -Method Get
Write-Host ($response | ConvertTo-Json -Depth 5)
Wait-Continue "Zakoñczono GET u¿ytkownika o ID $newUserId"

# 4. PUT - Aktualizacja u¿ytkownika
Write-Host "`n4. PUT /api/users/$newUserId"

$updatedUserJson = @"
{
    "id": $newUserId,
    "firstname": "Janusz",
    "lastname": "Kowalski",
    "dateOfBirth": "1990-05-15T00:00:00",
    "sex": "Male",
    "customProperties": [
        { "id": $($response.CustomProperties[0].Id), "name": "Hobby", "value": "Koszykówka" },
        { "id": 0, "name": "UlubionyKolor", "value": "Niebieski" }
    ]
}
"@
Write-Host $updatedUserJson
Invoke-RestMethod -Uri "$baseUrl/$newUserId" -Method Put -Body ([System.Text.Encoding]::UTF8.GetBytes($updatedUserJson)) -ContentType "application/json"
Write-Host "U¿ytkownik zaktualizowany."
Wait-Continue "Zakoñczono PUT (aktualizacja u¿ytkownika)"

# 5. GET - Pobranie zaktualizowanego u¿ytkownika
Write-Host "`n5. GET /api/users/$newUserId"
$response = Invoke-RestMethod -Uri "$baseUrl/$newUserId" -Method Get
Write-Host ($response | ConvertTo-Json -Depth 5)
Wait-Continue "Zakoñczono GET u¿ytkownika o ID $newUserId"

# 6. DELETE - Usuniêcie u¿ytkownika
Write-Host "`n6. DELETE /api/users/$newUserId"
Invoke-RestMethod -Uri "$baseUrl/$newUserId" -Method Delete
Write-Host "U¿ytkownik usuniêty."
Wait-Continue "Zakoñczono DELETE (usuniêcie u¿ytkownika)"

# 7. GET - Sprawdzenie czy u¿ytkownik zosta³ usuniêty
Write-Host "`n7. GET /api/users/$newUserId"
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/$newUserId" -Method Get
    Write-Host ($response | ConvertTo-Json -Depth 5)
} catch {
    Write-Host "U¿ytkownik nie istnieje (404)."
}
Wait-Continue "Test zakoñczony"
