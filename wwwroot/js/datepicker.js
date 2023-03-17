$(document).ready(function () {
    var notAvailable = ["2023-03-19", "2023-03-15", "2023-03-15", "2023-03-21"];
    var startDate = null;
    var endDate = null;
    var totalCost = 0;
    var startAsDateTime = 0;
    var endDateAsTime = 0;
    var oneDayAsTime = 1000 * 3600 * 24;

    $("#datepicker").datepicker({
        dateFormat: "yy-mm-dd",
        multidate: true,
        beforeShowDay: function (date) {
            var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
            if (notAvailable.indexOf(string) != -1) {
                return [false];
            }
            if ((startDate != null) && (endDate != null) && (startDate != endDate)) {
                var currentDateAsTime = new Date(date).getTime();
                if ((currentDateAsTime >= startAsDateTime) && (currentDateAsTime <= endDateAsTime)) {
                    return [true, 'highlight'];
                }
            }
            return [true, ''];
        },
        onSelect: function (date) {
            if (startDate === null) {
                startDate = date;
                endDate = date;
            } else if (date <= startDate) {
                startDate = date;
            } else if (date >= startDate && date <= endDate) {
                startDate = date;
            } else if (date >= startDate && date >= endDate) {
                endDate = date;
            }
            startAsDateTime = new Date(startDate).getTime() - oneDayAsTime;
            endDateAsTime = new Date(endDate).getTime();

            var timeBetween = endDateAsTime - startAsDateTime;
            var daysBetween = timeBetween / (1000 * 60 * 60 * 24);
            var daysCount = daysBetween;

            $("#RentHeaderDto_TotalCost").attr('value', daysCount * parseFloat($("#CarDto_DayRentalPrice").val()));
            $("#RentHeaderDto_TotalCost").change();
            $("#RentHeaderDto_StartDate").attr('value', startDate);
            $("#RentHeaderDto_EndDate").attr('value', endDate);

        }
    });
});