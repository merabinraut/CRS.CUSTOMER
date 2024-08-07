const urlPattern = /^(?:\w+:)?\/\/([^\s.]+\.\S{2}|localhost[:?\d]*)\S*$/;
function IsValidURL(URL) {
    return urlPattern.test(URL);
}

function IsSameDomain(url) {
    const currentDomain = window.location.hostname;
    try {
        const urlObject = new URL(url);
        const urlDomain = urlObject.hostname;
        return urlDomain === currentDomain || urlDomain === 'localhost';
    } catch (error) {
        return false;
    }
}

//function CheckIfHasRedirectURL(Data, PostRequestData) {
//    if (Data != null && Data.Code == 999 && Data.RedirectURL && IsValidURL(Data.RedirectURL) && IsSameDomain(Data.RedirectURL)) {
//        window.location.href = Data.RedirectURL;
//        window.location.replace(Data.RedirectURL);
//        window.history.replaceState(null, '', Data.RedirectURL);
//        return;
//    }
//}

function CheckIfHasRedirectURL(Data, PostRequestData = "") {
    if (Data != null && Data.Code == 999 && Data.RedirectURL && IsValidURL(Data.RedirectURL) && IsSameDomain(Data.RedirectURL)) {
        if (PostRequestData != "" || PostRequestData != null) {
            //const queryParams = new URLSearchParams(PostRequestData).toString();
            const redirectURLWithParams = `${Data.RedirectURL}%26PostURL=${encodeURIComponent(PostRequestData)}`;
            window.location.href = redirectURLWithParams;
            window.location.replace(redirectURLWithParams);
            window.history.replaceState(null, '', redirectURLWithParams);
            return;
        }
        else {
            window.location.href = Data.RedirectURL;
            window.location.replace(Data.RedirectURL);
            window.history.replaceState(null, '', Data.RedirectURL);
            return;
        }
    }
}

function CheckIfHasRedirectFunction() {
    var QueryData = ParseQueryString(window.location.href);
    var IsRedirectURL = QueryData['IsRedirectURL'];
    var FunctionName = QueryData['FunctionName'];

    if (IsRedirectURL && FunctionName && typeof window[FunctionName] === 'function') {
        var queryString = window.location.search.substring(1);
        var startIndex = queryString.indexOf("IsRedirectURL=true") + "IsRedirectURL=true".length + 1;
        var remainingParams = queryString.substring(startIndex);

        var allParams = ParseQueryString(remainingParams);

        const paramsToIgnore = ['IsRedirectURL', 'FunctionName'];
        const params = Object.fromEntries(
            Object.entries(allParams).filter(([key]) => !paramsToIgnore.includes(key))
        );

        window[FunctionName].apply(null, Object.values(params));
        var urlString = window.location.href;
        var index = urlString.indexOf('IsRedirectURL=true&FunctionName');
        var filteredUrl = urlString.substring(0, index);
        //history.replaceState({}, document.title, filteredUrl);
        window.history.replaceState(null, '', filteredUrl);
    }
}

function ParseQueryString(queryString) {
    var params = {};
    var queryStringWithoutQuestionMark = queryString.startsWith('?') ? queryString.substring(1) : queryString;
    var keyValuePairs = queryStringWithoutQuestionMark.split(/[?&]/);

    keyValuePairs.forEach(function (keyValuePair) {
        var pair = keyValuePair.split('=');
        var key = decodeURIComponent(pair[0]);
        var value = pair.length > 1 ? decodeURIComponent(pair[1]) : null;
        params[key] = value;
    });
    return params;
}

