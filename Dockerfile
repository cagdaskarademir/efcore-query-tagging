#docker create the build instance
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY /. ./

#restore
# performance issue : RUN dotnet restore

# build with configuration
RUN dotnet build -c Release

RUN dotnet publish -c Release -o /src/output

# create the runtime instance
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /src/output .

ENTRYPOINT ["dotnet", "EfCoreTagging.dll"]
