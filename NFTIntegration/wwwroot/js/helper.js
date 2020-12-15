"use strict";
function downloadFromByteArray(options) {
    // Convert base64 string to numbers array.
    const numArray = atob(options.byteArray).split('').map(c => c.charCodeAt(0));
    // Convert numbers array to Uint8Array object.
    const uint8Array = new Uint8Array(numArray);
    // Wrap it by Blob object.
    const blob = new Blob([uint8Array], { type: options.contentType });
    // Create "object URL" that is linked to the Blob object.

    if (window.navigator && window.navigator.msSaveOrOpenBlob) //For IE
    {
        window.navigator.msSaveOrOpenBlob(blob, options.fileName);
    }
    else {
        const url = URL.createObjectURL(blob);
        // Invoke download helper function that implemented in 
        // the earlier section of this article.
        downloadFromUrl({ url: url, fileName: options.fileName });
        // At last, release unused resources.
        URL.revokeObjectURL(url);
    }
}

function downloadFromUrl(options) {
    var _a;
    const anchorElement = document.createElement('a');
    anchorElement.href = options.url;
    anchorElement.download = (_a = options.fileName) !== null && _a !== void 0 ? _a : '';
    anchorElement.click();
    anchorElement.remove();
}

function BlazorScrollToId(name) {
    const element = document.getElementsByName(name)[0];
    if (element instanceof HTMLElement) {
        element.scrollIntoView({
            behavior: "smooth",
            block: "start",
            inline: "nearest"
        });
    }
}
 