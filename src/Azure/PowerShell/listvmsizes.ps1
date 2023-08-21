$location = 'west europe'

$rgroupName = 'vmresourcegrp1'
Get-AzureRmResourceGroup
#Get-AzureRmVMSize -Location $location | Where-Object { $_.NumberOfCores -eq 1 }
#Get-AzureRmVM
#Stop-AzureRmVM -ResourceGroupName $rgroupName -Name myVM -Force
Start-AzureRmVM -ResourceGroupName $rgroupName -Name myVM