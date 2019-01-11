using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class ContactContext
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly AppSettings _appSetting;
        //private readonly IMongoCollection<ContactBook> _contactBook;
        
        public ContactContext(IOptionsSnapshot<AppSettings> appSetting)
        {
            _appSetting = appSetting.Value;
           _mongoClient = new MongoClient(_appSetting.MongoConnectionContact);
            _mongoDatabase = _mongoClient.GetDatabase(_appSetting.MongoDatabaseContact); 
        }

        private void CheckAndCreateCollection(string collectionName)
        {
            var collectionList = _mongoDatabase.ListCollections().ToList();
            if (collectionList.Where(doc => doc.GetValue("name").AsString.Equals(collectionName)).Count() <= 0)
            {
                _mongoDatabase.CreateCollection(collectionName);
            }
        }

        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                CheckAndCreateCollection("ContactBooks");
                return _mongoDatabase.GetCollection<ContactBook>("ContactBooks");
            }
        }

        public IMongoCollection<ContactApplyRequest> ContactApplyRequests
        {
            get
            {
                CheckAndCreateCollection("ContactApplyRequests");
                return _mongoDatabase.GetCollection<ContactApplyRequest>("ContactApplyRequests");
            }
        }
    }
}
