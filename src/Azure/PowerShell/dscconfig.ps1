Configuration IIS {
    
        # Import the module that contains the resources we're using.
        #Import-DscResource -ModuleName PsDesiredStateConfiguration
    
        # The Node statement specifies which targets this configuration will be applied to.
        Node 'localhost' {
    
            # The first resource block ensures that the Web-Server (IIS) feature is enabled.
            WindowsFeature WebServer {
                Ensure = "Present"
                Name   = "Web-Server"
            }
        }
    }