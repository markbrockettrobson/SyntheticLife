FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /SyntheticLife
COPY * ./

RUN dotnet build -c Release -o /app
RUN dotnet test