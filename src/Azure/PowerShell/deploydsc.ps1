$rgroupName = 'vmresourcegrp1'
$stargeAccountName = 'mayvmstoragename'

# https://docs.microsoft.com/en-us/powershell/module/azurerm.compute/set-azurermvmdscextension?view=azurermps-4.3.1
# https://docs.microsoft.com/en-us/powershell/dsc/quickstart

Publish-AzureRmVMDscConfiguration -ConfigurationPath .\dscconfig.ps1 -ResourceGroupName $rgroupName -StorageAccountName $stargeAccountName

Set-AzureRmVMDscExtension -Version 2.21 -ResourceGroupName $rgroupName -VMName myVM `
-ArchiveStorageAccountName $stargeAccountName -ArchiveBlobName dscconfig.ps1.zip -AutoUpdate:$true -ConfigurationName IIS