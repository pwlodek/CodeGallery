https://docs.microsoft.com/en-us/azure/virtual-machines/windows/resize-vm

$rgname = "<resourceGroupName>"
$vmname = "<vmName>"
Stop-AzureRmVM -ResourceGroupName $rgname -VMName $vmname -Force
$vm = Get-AzureRmVM -ResourceGroupName $rgname -VMName $vmname
$vm.HardwareProfile.VmSize = "<newVMSize>"
Update-AzureRmVM -VM $vm -ResourceGroupName $rgname
Start-AzureRmVM -ResourceGroupName $rgname -Name $vmname