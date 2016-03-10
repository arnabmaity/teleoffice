//var currentTime = new Date ( );
//var currentHours = currentTime.getHours ( );
//var currentMinutes = currentTime.getMinutes ( );
//var currentSeconds = currentTime.getSeconds ( );
//currentMinutes = ( currentMinutes < 10 ? "0" : "" ) + currentMinutes;
//currentSeconds = ( currentSeconds < 10 ? "0" : "" ) + currentSeconds;
//var timeOfDay = ( currentHours < 12 ) ? "AM" : "PM";
//currentHours = ( currentHours > 12 ) ? currentHours - 12 : currentHours;
//currentHours = ( currentHours == 0 ) ? 12 : currentHours;

//var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " " + timeOfDay;
//<span id="clock">&nbsp;</span>
//document.getElementById("clock").firstChild.nodeValue = currentTimeString;



//<script type="text/javascript">
//<!--

function updateClock ( )
{
  var currentTime = new Date ( );

  var currentHours = currentTime.getHours ( );
  var currentMinutes = currentTime.getMinutes ( );
  //var currentHours = 0
 // var currentMinutes = 0
  var currentSeconds = currentTime.getSeconds ( );

  // Pad the minutes and seconds with leading zeros, if required
  currentMinutes = ( currentMinutes < 10 ? "0" : "" ) + currentMinutes;
  currentSeconds = ( currentSeconds < 10 ? "0" : "" ) + currentSeconds;

  // Choose either "AM" or "PM" as appropriate
  var timeOfDay = ( currentHours < 12 ) ? "AM" : "PM";

  // Convert the hours component to 12-hour format if needed
  currentHours = ( currentHours > 12 ) ? currentHours - 12 : currentHours;

  // Convert an hours component of "0" to "12"
  //currentHours = ( currentHours == 0 ) ? 12 : currentHours;

  // Compose the string for display
  var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " " + timeOfDay;

  // Update the time display
  document.getElementById("clock").firstChild.nodeValue = currentTimeString;
  
  
  }

// -->
//</script>


//<body onload="updateClock(); setInterval('updateClock()', 1000 )">