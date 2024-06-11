/*Alert button*/
$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

document.addEventListener("DOMContentLoaded", function() {
    flatpickr("#datetimepicker", {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
        time_24hr: true,
        minTime: "09:00",
        maxTime: "18:00"
    });
});
