using System;
using System.Windows;
using Caliburn.Core;
using CompositeWpfDemo.Infrastructure.Services;
using Microsoft.Practices.Composite.Logging;

namespace CompositeWpfDemo.Infrastructure.Startup
{
    public abstract class CaliburnBootstrapper
    {
        private readonly ILoggerFacade m_LoggerFacade = new TextLogger();
        private readonly IApplication m_Application;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaliburnBootstrapper"/> class.
        /// </summary>
        /// <param name="application">Instance representing current application.</param>
        protected CaliburnBootstrapper(IApplication application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            m_Application = application;
        }

        #region Public Members

        /// <summary>
        /// Gets the default <see cref="ILoggerFacade"/> for the application.
        /// </summary>
        /// <value>A <see cref="ILoggerFacade"/> instance.</value>
        protected virtual ILoggerFacade LoggerFacade
        {
            get { return m_LoggerFacade; }
        }

        /// <summary>
        /// Gets the <see cref="IApplication"/> instance.
        /// </summary>
        public IApplication Application
        {
            get { return m_Application; }
        }

        /// <summary>
        /// Gets the <see cref="IContainerFacade"/> instance.
        /// </summary>
        public IContainerFacade Container { get; private set; }

        /// <summary>
        /// Gets the Caliburn's container.
        /// </summary>
        public IContainer CaliburnContainer { get; private set; }

        /// <summary>
        /// Runs the bootstrapper sequence.
        /// </summary>
        public void Run()
        {
            var logger = LoggerFacade;
            if (logger == null)
            {
                throw new InvalidOperationException("LoggerFacade is null.");
            }

            logger.Log("Creating container.", Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException("Container is null.");
            }

            logger.Log("Creating Caliburn container.", Category.Debug, Priority.Low);
            CaliburnContainer = CreateCaliburnContainer();
            if (CaliburnContainer == null)
            {
                throw new InvalidOperationException("CaliburnContainer is null.");
            }

            logger.Log("Configuring Caliburn.", Category.Debug, Priority.Low);
            var core = CaliburnFramework.ConfigureCore(CaliburnContainer);
            ConfigureCaliburn(core);

            logger.Log("Registering services.", Category.Debug, Priority.Low);
            RegisterServices();

            logger.Log("Initializing services.", Category.Debug, Priority.Low);
            InitializeServices();

            logger.Log("Registering presenters.", Category.Debug, Priority.Low);
            RegisterPresenters();

            logger.Log("Registering application views.", Category.Debug, Priority.Low);
            RegisterViews();

            logger.Log("Final configuration before starting Caliburn.", Category.Debug, Priority.Low);
            BeforeStart(core);

            logger.Log("Starting Caliburn.", Category.Debug, Priority.Low);
            core.Start();

            logger.Log("Finishing setting up the application.", Category.Debug, Priority.Low);
            AfterStart();
        }

        #endregion

        #region Protected Create Methods

        protected abstract IContainerFacade CreateContainer();

        protected abstract IContainer CreateCaliburnContainer();

        protected abstract DependencyObject CreateShell();

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Configures the Caliburn framework.
        /// </summary>
        /// <param name="configuration">The core of Caliburn.</param>
        protected virtual void ConfigureCaliburn(CoreConfiguration configuration)
        {
        }

        protected virtual void RegisterServices()
        {
        }

        protected virtual void InitializeServices()
        {
        }

        protected virtual void RegisterPresenters()
        {
        }

        protected virtual void RegisterViews()
        {
        }

        /// <summary>
        /// Configures additional modules befores the starting the Caliburn framework.
        /// </summary>
        /// <param name="core">The core of Caliburn.</param>
        protected virtual void BeforeStart(CoreConfiguration core)
        {
        }

        /// <summary>
        /// Configures application after Caliburn framework has started.
        /// </summary>
        protected virtual void AfterStart()
        {
        }

        #endregion
    }
}