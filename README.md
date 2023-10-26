# ImageHubAPI

The project is an ASP.NET Core Web API for managing users and their roles.
<h4>The project implements the following:</h4>
<ol>
   <li>API Methods</li>
   <li>API documentation using Swagger.</li>
</ol>
<details>
<summary><h4>API methods:</h4></summary>

<ol> 
   <li><b>Registration:</b>
       <ul>
         <li>HTTP method: POST</li>
         <li>Path: /api/Account/Registration</li>
         <li>Description: The method allows you to register a new user.</li>
       </ul>
   </li>
   <li><b>Login:</b>
       <ul>
         <li>HTTP method: POST</li>
         <li>Path: /api/Account/Login</li>
         <li>Description: The method allows to log in.</li>
       </ul>
   </li>
   <li><b>AddFriend:</b>
       <ul>
         <li>HTTP method: POST</li>
         <li>Path: /api/User/AddFriend</li>
         <li>Description: The method allows to add a friend.</li>
       </ul>
   </li>
   <li><b>UploadImg:</b>
       <ul>
         <li>HTTP method: POST</li>
         <li>Path: /api/User/UploadImg</li>
         <li>Description: The method allows to upload images.</li>
       </ul>
   </li>
   <li><b>GetUserImg:</b>
       <ul>
         <li>HTTP method: Get</li>
         <li>Path: /api/User/GetUserImg</li>
         <li>Description: The method allows to get the list of user images</li>
       </ul>
   </li>
   <li><b>GetFriendImg:</b>
       <ul>
         <li>HTTP method: Get</li>
         <li>Path: /api/User/GetFriendImg</li>
         <li>Description: The method allows to get the list of friend images</li>
       </ul>
   </li>
   <li><b>GetUser:</b>
       <ul>
         <li>HTTP method: Get</li>
         <li>Path: /api/User/GetUser</li>
         <li>Description: The method allows to get the user by email</li>
       </ul>
   </li>
</ol>
</details>

**Access to documentation:**
<ol>
   <li>Launch the API</li>
   <li>Go to https://localhost:port/swagger/index.html in your browser, where "port" is the port the project is running on.</li>
</ol>

<b>Installation and use instructions:</b>
<ol>
   <li>Clone the repository.</li>
   <li>Go to the project directory.</li>
   <li>Make sure you have the .NET SDK installed. If not, install it from the official .NET site: https://dotnet.microsoft.com/download/dotnet</li>
   <li>Restore the project dependencies using the command: <b><i>dotnet restore</i></b></li>
   <li>Run the API using the command: <b><i>dotnet run</i></b></li>
</ol>
