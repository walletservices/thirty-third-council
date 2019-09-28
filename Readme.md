# 33rd Council - For Improvement Service Readme
## Introduction 
This read me has been created to document the functionality of the 33rd council website.  

## Logging in
Logging into the 33rd council is only possible via the my account idP.  

## Home Page
The home page allows the logged in user to start three processes.
*  The first process allows the user to apply for a basic blue badge.
* The second process allows the user to use claims that were previously populated in process 1 in this process.  
	* This process can only be started after the first process is completed
* The third process allows the user to use the attestations previously provided by the authoriser of process 1 to remove the requirement to provide the sensitive data again. 
	* This process can only be started after the first process is completed. 

Please note the current implementation does not prevent users starting process 2 and 3 before process 1 has been completed. 

## Process Page
A basic implementation of the process page is also provided to allow the user/citizen to see the progress of each process that they have started. 

## Configuration required
The source code contains an appsettings.json which contains all configuration values required to start the three processes and request the correct claims and attestations.
````
"Authentication": {
    "AzureAdB2C": {
      "ClientId": "The client ID required to authenticate to Siccar",
      "ClientSecret": "The client secret required to authenticate to Siccar,
      "Tenant": "The Siccar tenant",
      "SignUpSignInPolicyId": "The sign up policy name",
      "RedirectUri": "The redirect URL for redirection after logging on",
      "Authority": "The authority required to authenticate to Siccar",
      "SaveToken": true,
      "ApiScopes": "Not currently used"
    }
  },
  "SiccarConfig": {
    "ProcessA": "Process A schema ID",
    "ProcessAVersion": "Process A schema version",
    "ProcessB": "Process B schema ID",
    "ProcessBVersion": "Process B schema ID",
    "ProcessC": "Process C schema ID",
    "ProcessCVersion": "Process C schema ID",

    "GetGetActionToLoad": "Endpoint to fetch actions",
    "GetCheckifActionIsStartable": "Endpoint to confirm if process can be started",
    "GetGetProcessesThatICanStart": "Endpoint to get startable processes",
    "PostStartProcess": "Endpoint to start process",
    "PostSubmitStep": "Endpoint to submit an action",
    "GetGetProgressReports": "Endpoint to get progress reports",
    "TokenEndpoint": "Endpoint to connect to the STS ",
    "ExpectedClaims": "Expected Claims required to start Process B ",
    "ExpectedAttestations": "Expected Attestations required to start Process C"
  }
````

### Things to note
#### Claims and attestations 
Expected claims and attestations must be a comma separated list without **spaces**
````
"ExpectedClaims" : "claim1,claim2,claim3" 
````
would be valid 
````
"ExpectedClaims" : "claim1, claim2, claim3" 
````
would only return the first claim from the STS.

Claims and attestations can be retrieved via the designer.  

#### Processes
Each of the three processes schema id and schema version are stored within the config. 
The processes which are being executed can be changed by updating the appsettings.
````
	"ProcessA": "Process A schema ID",
    "ProcessAVersion": "Process A schema version",
````
````
    "ProcessB": "Process B schema ID",
    "ProcessBVersion": "Process B schema ID",
````
````
    "ProcessC": "Process C schema ID",
    "ProcessCVersion": "Process C schema ID"
````

The pairs must be updated together or the correct schema and version will not be started. 

#### Changing the URL of the 33rd council
As the 33rd council is redirected to from the Siccar B2C; if the URL is changed the Azure AD B2C application redirect URL must be updated to reflect the new change. 

#### Starting The First Process
The first process directly calls Siccar and starts a process without extra information.  
![Twat](https://i.guim.co.uk/img/media/9b1fe28fc874386c2353c58485ebea3a62bf21f8/164_345_4889_2933/master/4889.jpg?width=620&quality=45&auto=format&fit=max&dpr=2&s=1bfbf7b2fca633d0a1185d6679a927f0)

The above sequence diagram demonstrates the interactions between the logged in user, the app and Siccar.

#### Starting Process B 
The second process is started via the STS.  The STS interrogates the users wallet for the claims (which are provided in the app settings of the app) and then redirects to Siccar passing the returned claims  via a secondary JWT token.  These extra claims are then added to the ledger when starting the process and then returned to the user for confirmation. 
![Twat](https://i.guim.co.uk/img/media/9b1fe28fc874386c2353c58485ebea3a62bf21f8/164_345_4889_2933/master/4889.jpg?width=620&quality=45&auto=format&fit=max&dpr=2&s=1bfbf7b2fca633d0a1185d6679a927f0)

#### Starting Process C
The third process is started via the STS.  The STS interrogates the users wallet for the attestations (which are provided in the app settings of the app) and then redirects to Siccar passing the returned attestations via a secondary JWT token.  These extra attestations are then added to the ledger when starting the process.  They are not returned to the user. 
![Twat](https://i.guim.co.uk/img/media/9b1fe28fc874386c2353c58485ebea3a62bf21f8/164_345_4889_2933/master/4889.jpg?width=620&quality=45&auto=format&fit=max&dpr=2&s=1bfbf7b2fca633d0a1185d6679a927f0)

#### Core differences between the STS call for claims and attestations
The claims call is initiated with the following parameters.
````
HttpMethod: Post
Endpoint: https://poc.dlt.test.myaccount.scot:8691/connect/token
Header: Authorization: Bearer <JWT token from Siccar>
Body <Form Url Encoded>
client_id: thirty-third-council
grant_type: wallettoclaims
token: <JWT token from Siccar>
scopes: <Scopes from App Settings> ... ExpectedClaims
````
The attestation call is initiated with the following parameters. 
````
HttpMethod: Post
Endpoint: https://poc.dlt.test.myaccount.scot:8691/connect/token
Header: Authorization: Bearer <JWT token from Siccar>
Body <Form Url Encoded>
client_id: thirty-third-council
grant_type: wallettoattestations
token: <JWT token from Siccar>
scopes: <Scopes from App Settings> ... ExpectedAttestations
````

#### Interesting URLS 
33rd Council : 
Designer: https://dev.design.pds-poc.test.myaccount.scot/
Agent Executor: https://dev.agent.pds-poc.test.myaccount.scot/
Citizen Executor: https://dev.citizen.pds-poc.test.myaccount.scot/
Siccar Swagger: https://poc.dlt.test.myaccount.scot/swagger/index.html


#### Issues
* Due to the nature of the POC the system currently does not log the user out of the 33rd council if their token expires. 
* It also does not update the token which means the session can only last for an hour. 
* Process B and C cannot be started via the client executor.   
