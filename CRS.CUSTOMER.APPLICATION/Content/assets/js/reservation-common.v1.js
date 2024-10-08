﻿function InitiateClubReservationFunction(ClubId, SelectedDate = "", SelectedHost = "", PostData = "") {
    EnableLoaderFunction();
    document.body.classList.add('body-no-scroll');
    if (document.getElementById('club-bottom-tab-id')) {
        document.getElementById('club-bottom-tab-id').style.display = 'none';
    }
    $.ajax({
        type: 'GET',
        async: true,
        url: '/ReservationManagementV3/InitiateClubReservationProcess',
        dataType: 'json',
        data: {
            ClubId,
            //SelectedDate: $('#date-id').val(),
            SelectedDate,
            SelectedHost
        },
        success: function (data) {
            CheckIfHasRedirectURL(data, PostData);
            $("#stickey_id").css("display", "none");
            if (!data || data.Code !== 0) {
                toastr.info(data?.Message);
                DisableLoaderFunction();
                return false;
            }
            $('#clubreservationpartialview-id').html(data.PartialView);
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
                if (data.SelectedDate != null && data.SelectedDate != '') {
                    $('#date-id').val(data.SelectedDate.trim());
                    selectedDate = new Date(data.SelectedDate);
                }
                var dateList = JSON.parse(data.Dayoff);
                var formattedDates = [];
                dateList.forEach(function (dateString) {
                    if (dateString !== undefined) {
                        var dateParts = dateString.split('-');
                        if (dateParts.length === 3) {
                            var formattedDate = dateParts[0] + '-' + dateParts[1] + '-' + dateParts[2];
                            const year = currentDate.getFullYear();
                            const month = String(currentDate.getMonth() + 1).padStart(2, '0');
                            const day = String(currentDate.getDate()).padStart(2, '0');
                            const formattedCurrentDate = `${year}-${month}-${day}`;
                            if (formattedDate == formattedCurrentDate) {
                                $('#date-id').val('');
                            }
                            formattedDates.push(formattedDate);
                        }
                    }
                });
                var holidayDates = data.Dayoff;
                var timeIntervalBySelectedDate = JSON.parse(data.TimeIntervalBySelectedDate);
                var reservedTimeSlot = JSON.parse(data.ReservedTimeSlot);
                $("#datepicker").datepicker({
                    minDate: currentDate,
                    maxDate: maxDate,
                    defaultDate: selectedDate,
                    beforeShowDay: function (date) {
                        //var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                        //return [formattedDates.indexOf(string) == -1];
                        var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                        //if (date.getDay() === 0) { // Sunday
                        //    return [true, 'Sunday']; // Make Sunday unselectable
                        //}
                        //else if (formattedDates.indexOf(string) != -1) {
                        //    return [false]; // Unreservable date
                        //}
                        if (holidayDates.indexOf(string) != -1) {
                            return [false, 'Dayoff']; // Dayoff date
                        }
                        else {
                            return [true];
                        }
                    },

                    onSelect: function (dateText, inst) {
                        inst.inline = true; // Set datepicker to inline mode
                        $('#date-id').val(dateText.trim());
                        getTimeIntervalByDayWise(dateText, timeIntervalBySelectedDate, reservedTimeSlot);
                        initTimeFunction();
                    }
                });

                // Open the calendar by default
                $("#datepicker").datepicker("show");
                // Update selected date in datevalue element

            });
            initTimeFunction();
            initPeopleFunction();
            DisableLoaderFunction();
            document.body.classList.remove('body-no-scroll');
        },
        error: function (xhr, status, error) {
            if (document.getElementById('club-bottom-tab-id')) {
                document.getElementById('club-bottom-tab-id').style.display = '';
            }
            toastr.info("Something went wrong. Please try again later.");
            DisableLoaderFunction();
            document.body.classList.remove('body-no-scroll');
            return false;
        }
    });
}

function getTimeIntervalByDayWise(date, timeInterval, reservedTimeSlot) {
    var timeListHtml = '';
    var selectedDate = new Date(date);

    var firstElement = new Date(selectedDate.toDateString() + ' ' + timeInterval[0].Time);
    var lastElement = timeInterval[timeInterval.length - 1];
    var lastEntryTimeStr = lastElement.LastEntryTime;
    var endTimeStr = lastElement.Time;

    var startDisabledTime = parseTimeString(selectedDate, lastEntryTimeStr);
    var endDisabledTime = parseTimeString(selectedDate, endTimeStr);

    var filteredTimeInterval = timeInterval.filter(interval => interval.Time !== reservedTimeSlot.Time);

    filteredTimeInterval.forEach(function (item) {
        var itemTime = new Date(selectedDate.toDateString() + ' ' + item.Time);
        var currentTime = new Date();
        var disabledClassLabel = '';
        if (itemTime < currentTime) {
            disabledClassLabel = 'disabled';
        }
        if (itemTime > startDisabledTime || itemTime < firstElement) {
            disabledClassLabel = 'disabled';
        }
        //if (itemTime <= endDisabledTime) {
        //    disabledClassLabel = 'disabled';
        //}
        timeListHtml += '<div class="timeList ' + disabledClassLabel + ' h-[32px] px-3 py-1 text-[#666] text-xs flex justify-between items-center">';
        timeListHtml += '<div class="timeValue">' + item.Time + '</div>';
        timeListHtml += '<div class="activeTime">';
        timeListHtml += '<svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 12 12" fill="none">';
        timeListHtml += '<path d="M8.94643 3L4.88393 7.31494L3.05357 5.37077L2.25 6.22238L4.48214 8.59236L4.88393 9L5.28571 8.59236L9.75 3.8524L8.94643 3Z" fill="#666666" />';
        timeListHtml += '</svg>';
        timeListHtml += '</div>';
        timeListHtml += '</div>';
    });
    $('#timeListContainer').html(timeListHtml);
    initTimeFunction();
}
function parseTimeString(date, timeString) {
    var timeParts = timeString.split(':');
    var hours = parseInt(timeParts[0], 10);
    var minutes = parseInt(timeParts[1], 10);
    var newDate = new Date(date);
    newDate.setHours(hours, minutes, 0, 0);
    return newDate;
}
/////////////////////////////////////////////////////////////////////// Time JS ///////////////////////////////////////////////////////////////////////
function initTimeFunction() {
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
/////////////////////////////////////////////////////////////////////// Time JS ///////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////// People JS ///////////////////////////////////////////////////////////////////////
function initPeopleFunction() {
    var showPeopleLists = document.querySelectorAll('.showPeopleList');
    showPeopleLists.forEach(function (showPeopleList) {
        showPeopleList.addEventListener('click', function (event) {
            var timeList = showPeopleList.nextElementSibling;
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
        showPeopleLists.forEach(function (showPeopleList) {
            var timeList = showPeopleList.nextElementSibling;
            // Check if the click target is not the showTimeList, its timeList, or their descendants and close the timeList if it's open
            if (!showPeopleList.contains(event.target) && !timeList.contains(event.target) && timeList.style
                .display === "block") {
                timeList.style.display = "none";
            }
        });
    });

    document.querySelectorAll('.peopleList').forEach(item => {
        item.addEventListener('click', event => {
            document.querySelectorAll('.peopleList').forEach(item => {
                item.classList.remove('active');
            });
            event.currentTarget.classList.add('active');
            // Store the value if timeList has active class
            if (event.currentTarget.classList.contains('active')) {
                const peopleValue = event.currentTarget.querySelector('.peopleValue').textContent;
                //console.log('Time value:', peopleValue.trim());
                var selectedPeopleDiv = document.getElementById("selected-noofpeople-id");
                selectedPeopleDiv.innerText = peopleValue.trim();
                $('#noofpeople-id').val(peopleValue.trim());

            }
            if (parseInt($("#selected-noofpeople-id").text()) > 3) {
                $("#waitingMessageWrapper").css("display", "block");
            }
            else {
                $("#waitingMessageWrapper").css("display", "none");
            }
        });
    });
}
/////////////////////////////////////////////////////////////////////// People JS ///////////////////////////////////////////////////////////////////////

function CloseInitiatedClubReservationFunction() {
    document.body.classList.remove('body-no-scroll');
    $("#stickey_id").css("display", "")
    var element = document.getElementById('drawer-date');
    element.classList.add('translate-y-full');
    var removeElement = document.getElementById("ui-datepicker-div")
    removeElement.style.setProperty("display", "none", "important");
    if (document.getElementById('club-bottom-tab-id')) {
        document.getElementById('club-bottom-tab-id').style.display = '';
    }
}

function SubmitClubReservationFunction() {
    var form = document.getElementById('club-reservation-id');
    var requiredFields = form.querySelectorAll('[required]');
    for (var i = 0; i < requiredFields.length; i++) {
        if (!requiredFields[i].value) {
            /*toastr.info('Please fill out all required fields.');*/
            toastr.info('日付が必要です');
            return; // Prevent form submission
        }
    }
    // If all required fields are filled, submit the form
    form.submit();
}

/*function EnableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'block';
    
    document.body.classList.add('no-scroll-loader');
}
function DisableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'none';
    document.body.classList.remove('no-scroll-loader');
}
*/
function preventDefault(event) {
    event.preventDefault();
}
function EnableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'flex';
    document.body.addEventListener('touchmove', preventDefault, { passive: false });
    document.body.classList.add('no-scroll-loader');
}
function DisableLoaderFunction() {
    document.getElementById('loader-id-v2').style.display = 'none';
    document.body.removeEventListener('touchmove', preventDefault, { passive: false });
    document.body.classList.remove('no-scroll-loader');
}
