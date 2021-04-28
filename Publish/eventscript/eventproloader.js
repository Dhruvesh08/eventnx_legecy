$(document).ready(function () {
    var baseUrl = "https://www.eventnx.com/";
   
    dynamicallyLoadScript();
    function dynamicallyLoadScript() {
        const scriptPromise = new Promise((resolve, reject) => {
            const script = document.createElement('script');
            document.body.appendChild(script);
            script.onload = resolve;
            script.onerror = reject;
            script.async = true;
            script.src = baseUrl + 'eventscript/eventregistration.js';

        });

        scriptPromise.then(() => {
            var script = document.createElement("script");
            script.src = baseUrl + "eventscript/eventregistration.renderer.js";
            document.head.appendChild(script);

            var link = document.createElement("link");
            link.rel = "stylesheet";
            link.href = baseUrl + "eventscript/SubmitDataEvent.css";
            document.head.appendChild(link);
            
        });
    }

    
});