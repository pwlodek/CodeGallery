Try
{
    Get-AzureRmContext
}
Catch
{
    if ($_ -like "*Login-AzureRmAccount to login*") {
      Login-AzureRmAccount
    }
}

Select-AzureRmSubscription -SubscriptionId '2212db77-facb-4d17-973f-5d8c9eb9cc93'

$rgroupName = 'vmresourcegrp1'
$location = 'West Europe'
$stargeAccountName = 'myvmstorage'
New-AzureRmResourceGroup -Name $rgroupName -Location $location

Get-AzureRmVMSize -Location $location

Write-Host 'Creating subnest'
# Create a subnet configuration
$subnetConfig1 = New-AzureRmVirtualNetworkSubnetConfig -Name mySubnet -AddressPrefix 192.168.1.0/24
$subnetConfig2 = New-AzureRmVirtualNetworkSubnetConfig -Name mySubnet -AddressPrefix 192.168.2.0/24

Write-Host 'Creating virtual network'
# Create a virtual network
$vnet = New-AzureRmVirtualNetwork -ResourceGroupName $rgroupName -Location $location `
    -Name MYvNET -AddressPrefix 192.168.0.0/16 -Subnet $subnetConfig1

    Write-Host 'Creating public IP address'
# Create a public IP address and specify a DNS name
$pip = New-AzureRmPublicIpAddress -ResourceGroupName $rgroupName -Location $location `
    -AllocationMethod Static -IdleTimeoutInMinutes 4 -Name "mypublicdns$(Get-Random)"

    Write-Host 'Creating network securty group rules'
# Create an inbound network security group rule for port 3389
$nsgRuleRDP = New-AzureRmNetworkSecurityRuleConfig -Name myNetworkSecurityGroupRuleRDP  -Protocol Tcp `
-Direction Inbound -Priority 1000 -SourceAddressPrefix * -SourcePortRange * -DestinationAddressPrefix * `
-DestinationPortRange 3389 -Access Allow

# Create an inbound network security group rule for port 80
$nsgRuleWeb = New-AzureRmNetworkSecurityRuleConfig -Name myNetworkSecurityGroupRuleWWW  -Protocol Tcp `
-Direction Inbound -Priority 1001 -SourceAddressPrefix * -SourcePortRange * -DestinationAddressPrefix * `
-DestinationPortRange 80 -Access Allow

Write-Host 'Creating network security group'
# Create a network security group
$nsg = New-AzureRmNetworkSecurityGroup -ResourceGroupName $rgroupName -Location $location `
-Name myNetworkSecurityGroup -SecurityRules $nsgRuleRDP,$nsgRuleWeb    

Write-Host 'Creating network interface'
# Create a virtual network card and associate with public IP address and NSG
$nic = New-AzureRmNetworkInterface -Name myNic -ResourceGroupName $rgroupName -Location $location `
-SubnetId $vnet.Subnets[0].Id -PublicIpAddressId $pip.Id -NetworkSecurityGroupId $nsg.Id

# Storage
Write-Host 'Creating storage account'
$storageAccount = New-AzureRmStorageAccount -ResourceGroupName $rgroupName -Name $stargeAccountName -SkuName 'Standard_LRS'  -Location $location
$OSDiskUri = $storageAccount.PrimaryEndpoints.Blob.ToString() + "vhds/diskname.vhd"

# Define a credential object
$cred = Get-Credential

# Create a virtual machine configuration
$vmConfig = New-AzureRmVMConfig -VMName myVM -VMSize Basic_A1 | `
    Set-AzureRmVMOperatingSystem -Windows -ComputerName myVM -Credential $cred | `
    Set-AzureRmVMSourceImage -PublisherName MicrosoftWindowsServer -Offer WindowsServer `
    -Skus 2016-Datacenter -Version latest | Add-AzureRmVMNetworkInterface -Id $nic.Id | `
    Set-AzureRmVMOSDisk -Name 'osdiskname' -VhdUri $OSDiskUri -CreateOption FromImage | `
    Set-AzureRmVMBootDiagnostics -StorageAccountName $stargeAccountName

    Write-Host 'Creating VM'
New-AzureRmVM -ResourceGroupName $rgroupName -Location $location -VM $vmConfig

Write-Host 'Done!'

$del = Read-Host -Prompt 'Do you want to cleanup everything?'
if ($del -eq 'y') {
    Write-Output 'Deleting all stuff'
    
    #Remove-AzureRmAppServicePlan -Name 'TempServicePlan' -ResourceGroupName 'DefaultARMResourceGroup' -Force
    Remove-AzureRmResourceGroup -Name $rgroupName -Force
}

