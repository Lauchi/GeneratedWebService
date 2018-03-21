if (Test-Path Host/Eventstore.db) {
	Remove-Item -Force Host/Eventstore.db
}
cd SqlAdapter/
dotnet ef migrations remove -s ../Host/
dotnet ef migrations add InitialMigration -s ../Host/
dotnet ef database update -s ../Host/
cd ..