$(document).ready(function () {
    var notAvailable = [];
    var params = {};
    var queryString = window.location.search.substring(1);
    if (queryString) {
        var pairs = queryString.split('&');
        for (var i = 0; i < pairs.length; i++) {
            var pair = pairs[i].split('=');
            if (pair.length === 2) {
                var name = decodeURIComponent(pair[0]);
                var value = decodeURIComponent(pair[1]);
                params[name] = value;
            }
        }
    }

    var endpointUrl = 'https://localhost:44301/Customer/Shop/GetCarAvailability';
    if (Object.keys(params).length > 0) {
        endpointUrl += '?' + Object.keys(params).map(function (key) {
            return encodeURIComponent(key) + '=' + encodeURIComponent(params[key]);
        }).join('&');
    }

    var startDate = null;
    var endDate = null;
    var today = new Date();
    var maxDate = new Date();
    var numberOfMonthsAvailableInAdvance = 3;
    maxDate.setMonth(today.getMonth() + numberOfMonthsAvailableInAdvance);
    var clicksCount = 0;
    var startDateAsTime = 0;
    var endDateAsTime = 0;
    var oneDayAsTime = 1000 * 3600 * 24;

    $.get(endpointUrl, function (data) {
        notAvailable = JSON.parse(JSON.stringify(data.data));

        $("#datepicker").datepicker({
            minDate: today,
            maxDate: maxDate,
            dateFormat: "yy-mm-dd",
            multidate: true,
            beforeShowDay: function (date) {
                var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                if (notAvailable.indexOf(string) != -1) {
                    return [false];
                }
                if ((startDate != null) && (endDate != null) && (startDate != endDate)) {
                    var currentDateAsTime = new Date(date).getTime();
                    if ((currentDateAsTime >= startDateAsTime) && (currentDateAsTime <= endDateAsTime)) {
                        return [true, 'highlight'];
                    }
                }
                return [true, ''];
            },
            onSelect: function (date) {
                if (startDate === null && endDate === null) {
                    startDate = date;
                    endDate = date;
                }
                if (clicksCount % 2 == 0) {
                    if (date >= startDate && date < endDate) {
                        endDate = date;
                    } else if (date <= endDate) {
                        startDate = date;
                    }
                    else {
                        endDate = date;
                    }
                } else {
                    if (date <= startDate) {
                        endDate = startDate;
                        startDate = date;
                    } else if (date >= startDate && date <= endDate) {
                        startDate = date;
                    } else {
                        endDate = date;
                    }
                }
                startDateAsTime = new Date(startDate).getTime(); //have to decrement days by one, to highlight current day as picked aswell
                endDateAsTime = new Date(endDate).getTime();

                var rangeHasNotAvailableDates = false;
                if ((startDate != null) && (endDate != null)) {
                    for (var date = startDateAsTime; date <= endDateAsTime; date += oneDayAsTime) {
                        var fullDate = jQuery.datepicker.formatDate('yy-mm-dd', new Date(date));
                        if (notAvailable.indexOf(fullDate) != -1) {
                            rangeHasNotAvailableDates = true;
                            break;
                        }
                    }
                }

                if (rangeHasNotAvailableDates) {
                    endDateAsTime = 0;
                    startDateAsTime = 0;
                    startDate = null;
                    endDate = null;
                    $(this).datepicker('setDate', null);
                    rangeHasNotAvailableDates = false;
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'The selected range included not available days. Please select different range.',
                    });
                    return;
                }

                startDateAsTime -= oneDayAsTime; //have to decrement days by one, to higlight first day aswell
                var timeBetween = endDateAsTime - startDateAsTime;
                var daysBetween = timeBetween / (1000 * 60 * 60 * 24);
                var daysCount = daysBetween;

                $("#RentHeaderDto_TotalCost").attr('value', daysCount * parseFloat($("#CarDto_DayRentalPrice").val()));
                $("#RentHeaderDto_TotalCost").change();
                $("#RentHeaderDto_StartDate").attr('value', startDate);
                $("#RentHeaderDto_EndDate").attr('value', endDate);

                clicksCount += 1;
            }
        });
    });
});
