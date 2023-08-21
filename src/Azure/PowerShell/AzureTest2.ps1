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
Remove-AzureRmResourceGroup -Name 'Vms' -Force

Write-Host 'Done.'