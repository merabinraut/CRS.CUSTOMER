using CRS.CUSTOMER.APPLICATION.Hubs;
using CRS.CUSTOMER.APPLICATION.Models.NotificationManagement;
using Microsoft.AspNet.SignalR;
using System;
using System.IO;
using TableDependency.SqlClient;

namespace CRS.CUSTOMER.APPLICATION.Services
{
    public class SqlDependencyService
    {
        private SqlTableDependency<tbl_customer_notification> _tableDependency;
        private readonly IHubContext<NotificationHub> _hubContext;
        public SqlDependencyService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void StartListening(string connectionString)
        {
            _tableDependency = new SqlTableDependency<tbl_customer_notification>(connectionString);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(tbl_customer_notification)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender,
            TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<tbl_customer_notification> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var changedEntity = e.Entity;
                await _hubContext.Clients.All.SendNotification(changedEntity.NotificationBody);
            }
        }

        public void Dispose()
        {
            _tableDependency.Stop();
        }
    }
}

public class tbl_customer_notification
{
    public int notificationId { get; set; }
    public string NotificationBody { get; set; }
    public bool NotificationReadStatus { get; set; }
}