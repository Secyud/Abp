@echo off

set modules=features settings permissions tenants 
set uri=http://localhost:5062 

for %%a in (%modules%) do  (
	echo %%a 
	 cd ../../../%%a/src/Secyud.Abp.%%a.HttpApi.Client/
	 abp generate-proxy -t csharp -u %uri% --without-contracts -m %%a 
)


pause
