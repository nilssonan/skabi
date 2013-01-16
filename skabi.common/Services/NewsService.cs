﻿using System.Collections.Generic;
using skabi.data.DomainModel;
using skabi.data.Repository;
using skabi.data.Repository.Interfaces;


namespace skabi.common.Services
{

    public class NewsService : INewsService
    {

        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
            
           
        }

        public IEnumerable<News> GetTopFiveNews()
        {
            return _newsRepository.GetTopFiveNews();
        }
    }
}