function InitateRescheduleReservationFunction(clubId, reservedDate, noOfPeople, reservationId, time) {
    EnableLoaderFunction();
    document.body.classList.add('body-no-scroll');

    $.ajax({
        type: 'GET',
        async: true,
        url: '/ReservationHistoryManagementV2/InitateRescheduleReservationProcess',
        dataType: 'json',
        data: {
            clubId,
            reservedDate,
            noOfPeople,
            reservationId,
            time
        },
        success: function (response) {
            if (!response || response.Code !== 0) {
                toastr.info(response?.Message);
                DisableLoaderFunction();
                return false;
            }
            $('#clubreservation-reschedule-partialview').html(response.PartialView);
            jQuery(function ($) {
                $.datepicker.regional['ja'] = {
                    closeText: '閉じる',
                    prevText: '&#x3c;前',
                    nextText: '次&#x3e;',
                    currentText: '今日',
                    monthNames: ['1月', '2月', '3月', '4月', '5月', '6月',
                        '7月', '8月', '9月', '10月', '11月', '12月'
                    ],
                    monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月',
                        '7月', '8月', '9月', '10月', '11月', '12月'
                    ],
                    dayNames: ['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日'],
                    dayNamesShort: ['日', '月', '火', '水', '木', '金', '土',],
                    dayNamesMin: ['日', '月', '火', '水', '木', '金', '土',],
                    weekHeader: '週',
                    dateFormat: 'yy/mm/dd',
                    firstDay: 0,
                    isRTL: false,
                    showMonthAfterYear: true,
                    yearSuffix: '年'
                };
                $.datepicker.setDefaults($.datepicker.regional['ja']);
                var currentDate = new Date();
                var maxDate = new Date(currentDate.getTime() + (14 * 24 * 60 * 60 * 1000)); // 15 days from today
                var selectedDate = null; //= currentDate;
                if (response.SelectedDate != null && response.SelectedDate != '') {
                    $('#date-id').val(response.SelectedDate.trim());
                    selectedDate = new Date(response.SelectedDate);
                }
                var timeIntervalBySelectedDate = JSON.parse(response.TimeIntervalBySelectedDate);
                var reservedTimeSlot = JSON.parse(response.ReservedTimeSlot);
                $("#datepicker").datepicker({
                    minDate: currentDate,
                    maxDate: maxDate,
                    defaultDate: selectedDate,
                    beforeShowDay: function (date) {
                        var newDate = $.datepicker.formatDate('yy-mm-dd', date);
                        let responses = (newDate === response.SelectedDate.trim());
                        return [newDate === response.SelectedDate.trim()];
                    },
                    onSelect: function (dateText, inst) {
                        inst.inline = true; // Set datepicker to inline mode
                        $('#date-id').val(dateText.trim());
                        rescheduleInitTimeFunction();
                    }
                });

                // Open the calendar by default
                $("#datepicker").datepicker("show");
            });
            rescheduleInitTimeFunction();
            DisableLoaderFunction();
        },
        error: function (xhr, status, error) {
            DisableLoaderFunction();
            toastr.info("Something went wrong. Please try again later.");
            document.body.classList.remove('body-no-scroll');
            return false;
        }
    });
}

function rescheduleInitTimeFunction() {
    var showTimeLists = document.querySelectorAll('.showTimeList');
    showTimeLists.forEach(function (showTimeList) {
        showTimeList.addEventListener('click', function (event) {
            var timeList = showTimeList.nextElementSibling;
            if (timeList.style.display === "none" || timeList.style.display === "") {
                timeList.style.display = "block";
            } else {
                timeList.style.display = "none";
            }
            event.stopPropagation(); // Prevents the click event from bubbling up to the document
        });
    });

    document.addEventListener('click', function (event) {
        // Loop through each showTimeList
        showTimeLists.forEach(function (showTimeList) {
            var timeList = showTimeList.nextElementSibling;
            // Check if the click target is not the showTimeList, its timeList, or their descendants and close the timeList if it's open
            if (!showTimeList.contains(event.target) && !timeList.contains(event.target) && timeList.style
                .display === "block") {
                timeList.style.display = "none";
            }
        });
    });

    document.querySelectorAll('.timeList').forEach(item => {
        item.addEventListener('click', event => {
            if (event.currentTarget.classList.contains('disabled')) {
                return;
            }
            document.querySelectorAll('.timeList').forEach(item => {
                item.classList.remove('active');
            });
            event.currentTarget.classList.add('active');

            // Store the value if timeList has active class
            if (event.currentTarget.classList.contains('active')) {
                const timeValue = event.currentTarget.querySelector('.timeValue').textContent;
                var selectedTimeDiv = document.getElementById("selected-time-id");
                selectedTimeDiv.innerText = timeValue.trim();
                $('#time-id').val(timeValue.trim());
            }
        });
    });
}

function CloseInitiatedRescheduleReservationFunction() {
    document.body.classList.remove('body-no-scroll');
    $("#stickey_id").css("display", "")
    var element = document.getElementById('drawer-date');
    element.classList.add('translate-y-full');
    var removeElement = document.getElementById("ui-datepicker-div")
    removeElement.style.setProperty("display", "none", "important");
}

function SubmitRescheduleReservationFunction() {
    var form = document.getElementById('club-reschedule-reservation-id');
    var requiredFields = form.querySelectorAll('[required]');
    for (var i = 0; i < requiredFields.length; i++) {
        if (!requiredFields[i].value) {
            /*toastr.info('Please fill out all required fields.');*/
            toastr.info('日付が必要です');
            return; // Prevent form submission
        }
    }
    // If all required fields are filled, submit the form
    var token = getAntiForgeryToken();
    var tokenInput = document.createElement('input');
    tokenInput.type = 'hidden';
    tokenInput.name = token.name;
    tokenInput.value = token.value;
    // Append the token to the form
    form.appendChild(tokenInput);
    form.submit();
}

function getAntiForgeryToken() {
    var tokenName = "__RequestVerificationToken";
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    return { name: tokenName, value: token };
}

function EnableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'flex';
    //function preventDefault(e) {
    //    e.preventDefault();
    //}
    //document.body.addEventListener('touchmove', preventDefault, { passive: false });
    document.body.classList.add('no-scroll-loader');
}
function DisableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'none';
    //function preventDefault(e) {
    //    e.preventDefault();
    //}
    //document.body.removeEventListener('touchmove', preventDefault, { passive: false });
    document.body.classList.remove('no-scroll-loader');
}