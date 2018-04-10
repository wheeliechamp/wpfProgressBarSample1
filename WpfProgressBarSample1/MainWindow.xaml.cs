using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfProgressBarSample1
{
  /// <summary>
  /// MainWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : Window
  {
    private bool searchRun;
    private List<DeviceInfo> dinfos;

    public MainWindow()
    {
      InitializeComponent();
      this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
      // Window描画完了後の処理
      ContentRendered += (s, e) =>
      {
        var portSearch = Task.Run(() =>
        {
          var knownPorts = new List<string>();
          while (searchRun)
          {
            var portList = SerialPort.GetPortNames().ToList();
            foreach (var portName in portList)
            {
              // ポートの登録、重複登録はしない
              if (!knownPorts.Contains(portName.ToString()))
              {
                knownPorts.Add(portName.ToString());
                Debug.WriteLine("--- :: " + portName.ToString());
                // Open可能なポートを追加する
              }

              System.Threading.Thread.Sleep(1000);
              Debug.WriteLine(knownPorts.Count());
              Debug.WriteLine("AAA");
              // knownPortsをリストにバインド
              // ここでデバイスの数分ループしてバインド？
            }
          }
        });
        Task.Run(() =>
      {
        for (int i = 0; i <= 100; i++)
        {
          dinfos[0].DownLoad = i;
          System.Threading.Thread.Sleep(100);
        }
      });
      };
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.dinfos = new List<DeviceInfo>();

      var deviceInfo = new DeviceInfo
      {
        PortName = "",
        DeviceID = "1710001",
        DownLoad = 0,
        UpLoad = 10
      };
      dinfos.Add(deviceInfo);

      deviceInfo = new DeviceInfo
      {
        PortName = "",
        DeviceID = "1710002",
        DownLoad = 0,
        UpLoad = 20
      };
      dinfos.Add(deviceInfo);
      this.datagrid.ItemsSource = dinfos;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      //await Task.Run(() =>
      //{
        for (int i = 0; i <= 100; i++)
        {
          dinfos[1].DownLoad = i;
          System.Threading.Thread.Sleep(200);
        }
      //});
    }
  }

  public class DeviceInfo : INotifyPropertyChanged
  {
    public string PortName { get; set; }
    public string DeviceID { get; set; }
    public int UpLoad { get; set; }

    private int ProgressValue;
    public int DownLoad
    {
      get { return this.ProgressValue; }
      set
      {
        if (value != this.ProgressValue)
        {
          this.ProgressValue = value;
          NotifyPropertyChanged("DownLoad");
        }
      }
    }

    // INotifyPropertyChanged メンバ
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string PropertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
    }
  }
}
