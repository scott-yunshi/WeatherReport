const report = document.getElementById('report');
const sendWeatherRequestBtn = document.getElementById('sendWeatherRequestBtn');

//getWeatherReport();

sendWeatherRequestBtn.addEventListener('click', getWeatherReport);

async function getWeatherReport() {
	var city = document.getElementById('city').value;
	var country = document.getElementById('country').value;
	
	if (!city || city.length === 0 ||!country || country.length === 0) {
	    report.innerText = "City or Country cannot be empty";
	} else {
      let request = new XMLHttpRequest();
	  
      request.open("GET",`http://localhost:5000/api/weather?city=${city}&&country=${country}`);
	  request.setRequestHeader("WeatherReportAPIAuthKey", "key5");
      request.send();
      request.onload = () => {
		  console.log(request.status);
        if (request.status === 200) {
            report.innerText = JSON.parse(request.responseText)["weatherDescription"];
        } else if (request.status === 401){
			report.innerText = "Sorry, You don't have the visit Authorities.";
		}else if (request.status === 429){
			report.innerText = request.responseText;
		}else {
			report.innerText = JSON.parse(request.responseText)["Message"];
        }
      };
	}
}

