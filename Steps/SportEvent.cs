using FluentAssertions;
using NetAPorter.Base;
using NetAPorter.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NetAPorter.Steps
{
    [Binding]
    public sealed class SportEvent
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private AppSettings _appSettings;
        private readonly ApiClient _apiCLient;
        private MRDataResponse _mRDataResponse;
        public SportEvent(ScenarioContext scenarioContext, FeatureContext featureContext, AppSettings appSettings, ApiClient apiClient)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _appSettings = appSettings;
            _apiCLient = apiClient;
        }
        [Given(@"the Formula One End Point is called using Get Method")]
        public async Task GivenTheFormulaOneEndPointIsCalledUsingGetMethod()
        {
            _appSettings = _scenarioContext.Get<AppSettings>();
            _mRDataResponse = await _apiCLient.GetAsync<MRDataResponse>($"{_appSettings.TestHarness.URI}api/f1.json");
        }
        [Given(@"the Formula One End Point is called using Get Method with (.*)")]
        public async Task GivenTheFormulaOneEndPointIsCalledUsingGetMethodWithDriver_Json(string p0)
        {
            _appSettings = _scenarioContext.Get<AppSettings>();
            _mRDataResponse = await _apiCLient.GetAsync<MRDataResponse>($"{_appSettings.TestHarness.URI}api/f1/"+p0);
        }

        [Given(@"the Formula One End Point is called with season (.*) using Get Method")]
        public async Task GivenTheFormulaOneEndPointIsCalledWithUsingGetMethod(string season)
        {
            _appSettings = _scenarioContext.Get<AppSettings>();
            _mRDataResponse = await _apiCLient.GetAsync<MRDataResponse>($"{_appSettings.TestHarness.URI}api/f1/"+season+"/drivers.json");
        }


        [Then(@"Verify the Drivers in the table below")]
        public void ThenVerifyTheDriversInTheTableBelow(Table table)
        {
            var tableContent = table.CreateInstance<ObjectData>();
            for(int i = 0; i < Convert.ToInt32(_mRDataResponse.MRData.total); i++)
            {
                if (_mRDataResponse.MRData.DriverTable.Drivers[i].givenName == tableContent.driverId)
                {
                    _mRDataResponse.MRData.DriverTable.Drivers[i].givenName.Should().Contain(tableContent.givenName);
                    _mRDataResponse.MRData.DriverTable.Drivers[i].familyName.Should().Contain(tableContent.familyName);
                    _mRDataResponse.MRData.DriverTable.Drivers[i].dateOfBirth.Should().Contain(tableContent.dateOfBirth);
                    _mRDataResponse.MRData.DriverTable.Drivers[i].nationality.Should().Contain(tableContent.nationality);
                }

            }

        }
        [Then(@"Verify Formula One Race details")]
        public void ThenVerifyFRaceDetails()
        {
            _mRDataResponse.MRData.RaceTable.Races[1].raceName.Should().Be("Monaco Grand Prix");
        }


    }
}
