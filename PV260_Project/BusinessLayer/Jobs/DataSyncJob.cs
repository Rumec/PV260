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

        private Timer? timer;

        public DataSyncJob(IDataDownloader downloader, IDataSetService dataSetService, IFileUrlService fileUrlService)
        {
            _downloader = downloader;
            _dataSetService = dataSetService;
            _fileUrlService = fileUrlService;
        }

        public void Run()
        {
            timer = new Timer(new TimerCallback(DownloadAndSaveFile), null, 0, 24 * 60 * 60 * 1000);
        }

        public async void DownloadAndSaveFile(object? state)
        {
            try
            {
                var fileUrl = await _fileUrlService.GetLatest();
                Console.WriteLine($"fileUrl: {fileUrl?.Url}");
                if (fileUrl == null)
                {
                    Console.WriteLine("File url is not specifed, please set a file url for automatic downloads");
                    return;
                }
                DataSet dataSet = await _downloader.LoadCsvFile(fileUrl.Url);
                await _dataSetService.CreateDataSet(dataSet);
            }
            catch (DataSetAlreadyExistsException) {}
            catch (Exception e)
            {
                Console.WriteLine("Automatic data load failed...");
            }
        }

        public void Stop()
        {
            timer?.Dispose();
        }
    }
}
