
# 🎥 Azure Function App - Generate SAS URL for Blob Media

This project sets up an **Azure Function App** to generate **SAS (Shared Access Signature)** URLs for media files (e.g., `1.mp4`) stored in **Azure Blob Storage**, using **Azure Key Vault** for secrets and **Managed Identity** for secure access.

---

## ✅ What This Does

- 🎯 Accepts a filename via HTTP GET (e.g., `/generate-sas/1.mp4`)
- 🔐 Authenticates using a **Function Key**
- 📦 Accesses a blob inside the `images` container
- 🧾 Generates a time-limited **SAS URL**
- 🧑‍💻 Returns the secure link to the client for download or streaming

---

## 🛠️ Prerequisites

- Azure CLI installed
- Node.js & npm installed
- .NET SDK (for dotnet-isolated function runtime)
- Azure Functions Core Tools v4 installed

---

## 🚀 Deployment Steps

### 1. 🔐 Login to Azure

```bash
az login
2. 📦 Create Resource Group
bash
Copy
Edit
az group create --name rgshafeeq --location eastus2
3. ☁️ Create Storage Account
bash
Copy
Edit
az storage account create \
  --name scshafeeq \
  --location eastus2 \
  --resource-group rgshafeeq \
  --sku Standard_LRS
4. ⚙️ Create Function App (Consumption Plan)
bash
Copy
Edit
az functionapp create \
  --resource-group rgshafeeq \
  --consumption-plan-location eastus2 \
  --name functionshafeeq \
  --storage-account scshafeeq \
  --runtime dotnet-isolated \
  --functions-version 4
5. 🛠️ Set Function App Settings
bash
Copy
Edit
az functionapp config appsettings set \
  --name functionshafeeq \
  --resource-group rgshafeeq \
  --settings \
  AzureStorageConnectionString="..." \
  ContainerName="images" \
  KeyVaultUri="https://your-keyvault-name.vault.azure.net/"
6. 🔐 Enable Managed Identity
bash
Copy
Edit
az functionapp identity assign \
  --name functionshafeeq \
  --resource-group rgshafeeq
7. 🔑 Assign IAM Roles
Go to the Azure Portal or use CLI to assign the following roles:

Storage Blob Data Reader on the storage account

Key Vault Secrets User on the Key Vault

8. 💻 Install Azure Functions Core Tools
bash
Copy
Edit
sudo npm install -g azure-functions-core-tools@4 --unsafe-perm true
9. 🚀 Publish Function App
bash
Copy
Edit
func azure functionapp publish functionshafeeq
10. 🔑 Get Function Key
bash
Copy
Edit
az functionapp function keys list \
  --resource-group rgshafeeq \
  --name functionshafeeq \
  --function-name Function
📥 Example Usage
http
Copy
Edit
https://functionshafeeq.azurewebsites.net/api/generate-sas/1.mp4?code=YOUR_FUNCTION_KEY
/api/generate-sas/1.mp4: Your filename

code=...: Function key for authorization

📂 Folder Structure (Example)
pgsql
Copy
Edit
FunctionApp/
├── Function.cs
├── local.settings.json
├── host.json
├── Company.FunctionApp2.csproj
🧠 Key Concepts
Feature	Purpose
Azure Function	Handles logic to generate SAS token
Blob Storage	Stores your media files
SAS Token	Allows temporary access to a blob
Key Vault	Securely stores secrets (connection strings, etc.)
Managed Identity	Authenticates securely without exposing credentials
Function Key	Protects your function from unauthorized access