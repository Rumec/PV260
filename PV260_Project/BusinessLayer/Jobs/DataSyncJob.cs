using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DataLoading;
using DataLayer.Models;
using BusinessLayer.Services;
using BusinessLayer.Exceptions;


namespace BusinessLayer.Jobs
{
    public class DataSyncJob : IDataSyncJob
    {
        private readonly IDataDownloader _downloader;
        private readonly IDataSetService _dataSetService;
        private readonly IFileUrlService _fileUrlService;

        private const int DATA_SYNC_INTERVAL_IN_MS = 24 * 60 * 60 * 1000;

        private Timer? timer;

        public DataSyncJob(IDataDownloader downloader, IDataSetService dataSetService, IFileUrlService fileUrlService)
        {
            _downloader = downloader;
            _dataSetService = dataSetService;
            _fileUrlService = fileUrlService;
        }

        public void Run()
        {
            timer = new Timer(new TimerCallback(DownloadAndSaveFile), null, 0, DATA_SYNC_INTERVAL_IN_MS);
        }

        public async void DownloadAndSaveFile(object? state)
        {
            try
            {
                var fileUrl = await _fileUrlService.GetLatest();
                if (fileUrl is null)
                {
                    Console.WriteLine("Automatic download: file url is not specifed, please set a file url for automatic downloads in the config menu");
                    return;
                }
                DataSet dataSet = await _downloader.LoadCsvFile(fileUrl.Url);
                await _dataSetService.CreateDataSet(dataSet);
            }
            catch (DataSetAlreadyExistsException) {
                Console.WriteLine("Automatic download: data already downloaded");
            }
            catch (Exception)
            {
                Console.WriteLine("Automatic download: data load failed...");
            }
        }

        public void Stop()
        {
            timer?.Dispose();
        }
    }
}
