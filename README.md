
## download runtime
```sh
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-9.0.309-windows-x64-installer
```



## Build Project
```sh
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

หลังจาก build ไฟล์จะอยู่ใน bin/Release/net8.0/win-x64/
