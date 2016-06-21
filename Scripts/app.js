var url = 'http://instadateapi.zaichaopan.com/api/Region';

window.onload = function find() {
    var startPos;
    var latitude;
    var longitude
    var geoOptions = {
        enableHighAccuracy: true,
    }
    var geoError = function (position) {
        console.log('Error occurred. Error code: ' + error.code);
        // error.code can be:
        //   0: unknown error
        //   1: permission denied
        //   2: position unavailable (error response from location provider)
        //   3: timed out
    };
    var geoSuccess = function (position) {
        startPos = position;
        latitude = startPos.coords.latitude;
        longitude = startPos.coords.longitude;
        reverseGeoLocate(latitude, longitude)
    }
    navigator.geolocation.getCurrentPosition(geoSuccess,geoError,geoOptions);
}
function calculateDistances(origins, destinations) {
    var service = new google.maps.DistanceMatrixService();
    var d = $.Deferred();
    service.getDistanceMatrix(
        {
            origins: origins,
            destinations: destinations,
            travelMode: google.maps.TravelMode.DRIVING,
            unitSystem: google.maps.UnitSystem.METRIC,
            avoidHighways: false,
            avoidTolls: false
        },
        function (response, status) {
            if (status != google.maps.DistanceMatrixStatus.OK) {
                d.reject(status);
            } else {
                d.resolve(response);
            }
        });
    return d.promise();
}
function reverseGeoLocate(latitude,longitude)
{
    var originLatLng = latitude + "," + longitude;
    var destLatLng = $(".latLng")
    var distance = $(".distance");
    if (distance)
    {
        destLatLng.each(function (index) {
            calculateDistances([originLatLng], [$(this).text()])
               .done(function (response) {
                   var results = response.rows[0].elements;
                   if (!(results[0].status == "OK")) {
                       distance[index].innerHTML = "Distance Not Available";
                   }else {
                       distance[index].innerHTML = results[0].distance.text;
                   }
               })
               .fail(function (status) {
                   document.getElementById('result').innerHTML = 'An error occured. Status: ' + status;
               });
        console.log($(this).text());
    });
    }
    var editLatitude = $("#latitude");
    var editLongitude = $("#longitude");
    editLatitude.val(latitude);
    editLongitude.val(longitude);
    var reverGeo = 'http://maps.googleapis.com/maps/api/geocode/json?latlng='+latitude+','+longitude+'&sensor=false'
    var province;
    var country;
    var city;
    var parseProvince = 5;
    var parseCity = 3;
    var parseCountry = 6;
    var componentSelector = 0;
    function parseData(location)
    {
        console.log(location);
                province = location.results[componentSelector].address_components[parseProvince].long_name;
                country = location.results[componentSelector].address_components[parseCountry].long_name;
                city = location.results[componentSelector].address_components[parseCity].long_name;

    }
    $.getJSON(reverGeo,
        function (data) {
            if (data == null) {
                $('#clientsResult').text('clients not found.');
                return;
            }
            parseData(data);
            getClients(province,country,city);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#clientsResult').text('Find error: ' + err);
        });
}

function getClients(province,country,city)
{
    var editCountry = $("#countryId");
    var editState = $("#stateId");
    var editCity = $("#cityId");
    if (editCountry) {
        editCountry.val(country);
        editState.val(province);
        editCity.val(city);
      
    }
        $.getJSON(url + "/" + province,
            function (data) {
                if (data.clients.length == 0) {
                    $('#clientsResult').text('clients not found.');
                    return;
                }
                callbackClients(data);
            })
        .fail(
            function (jqueryHeaderRequest, textStatus, err) {
                $('#clientsResult').text('Find error: ' + err);
            });
}

function callbackClients(data) {
    //For Web API{
    var valTag = document.getElementById("clientsResult");
    valTag.innerHTML = "";
    var table = document.createElement("table");
    valTag.appendChild(table);
    table.className = "table table-striped table-hover table-responsive";
    var caption = document.createElement("caption");
    caption.innerHTML = data.province + ": " + data.totalPeople + " people" + "   " + "male: " + data.male + " " + "female: " + data.female;
    table.appendChild(caption);
    var tr1 = document.createElement("tr");
    table.appendChild(tr1);
    var th1 = document.createElement("th");
    th1.innerHTML = "Username";
    tr1.appendChild(th1);
    var th2 = document.createElement("th");
    th2.innerHTML = "Gender";
    tr1.appendChild(th2);
    var th3 = document.createElement("th");
    th3.innerHTML = "Age";
    tr1.appendChild(th3);

    data.clients.forEach(function (val) {
        var tr2 = document.createElement("tr");
        table.appendChild(tr2);
        var td1 = document.createElement("td");
        td1.innerHTML = val.Username;
        tr2.appendChild(td1);
        var td2 = document.createElement("td");
        td2.innerHTML = val.Gender;
        tr2.appendChild(td2);
        var td3 = document.createElement("td");
        var now = new Date();
        td3.innerHTML = parseInt((now - Date.parse(val.Age)) / (1000 * 60 * 60 * 24 * 365));
        tr2.appendChild(td3);
    });
}

