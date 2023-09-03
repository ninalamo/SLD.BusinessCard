# Development Environment Setup
## IDE Setup
 1. Download the [root certificate](https://1drv.ms/u/s!AtLRBtMGaV2wg60d_MxJQaUV1nuu2A?e=qzng4e). Right-click the file and select Install Certificate. In the Certificate Import Wizard, select `Local Machine` as the Store Location then click the Next button. In the next window, select `Place all certificates in the following store` then click the browse button. Select `Trusted Root Certification Authorities` in the Select Certificate Store window. Click Finish in the next window.![Certificate Import Wizard](https://i.imgur.com/TdBbfMv.png)
 2. Download the local [development certificate](https://1drv.ms/u/s!AtLRBtMGaV2wg60cOEDvZ4CIl85LUw?e=RvjEat) and save it to the `BusinessCard.API` folder.
 3. Reboot your machine to for the changes to take effect.
## Docker Setup
1. Install Docker Desktop. Follow the steps from [this page](https://docs.docker.com/desktop/install/windows-install/).
2. Download the Volume [backup file](https://1drv.ms/u/s!AtLRBtMGaV2wg60acpe9qnrBt7J1xQ?e=FlNH1U) and save it to your pc.
3. Install Volumes Backup & Share Extension in docker. ![Docker Extension](https://i.imgur.com/rsMGyOh.png)
4. Open the Backups Extension and click Import into new volume. ![Volumes Backup](https://i.imgur.com/trPkbM0.png)
5. Import the file and use `sqlserver` as volume name. ![Import Volume Backup](https://i.imgur.com/WkgcaTx.png)
6. Execute `docker network create bcard` in your Terminal to create the container network for the business card project.
7. Execute `docker pull mcr.microsoft.com/mssql/server:2022-latest` in your Terminal to download the SQL Server 2022 image.
8. Execute `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=hBsUhjburD6P^#" -p 1433:1433 --name sqlserver --hostname sqlserver -v sqlserver:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2022-latest`.
9. Open the project in Visual Studio. Change the debug profile to `Docker` and start debugging to create a container for the project in Docker. It will raise an exception. Ignore it for now and stop debugging. ![Debug Profile](https://i.imgur.com/03K3wQe.png)
10. Execute `docker network connect bcard sqlserver` in your Terminal to attach the sql server container to the bcard network.
11. Execute `docker network connect bcard BusinessCard.API` in your Terminal.