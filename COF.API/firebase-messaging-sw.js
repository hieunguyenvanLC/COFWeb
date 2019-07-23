importScripts('https://www.gstatic.com/firebasejs/3.9.0/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/3.9.0/firebase-messaging.js');

// Initialize the Firebase app in the service worker by passing in the
// messagingSenderId.
firebase.initializeApp({
    messagingSenderId: "651552319035"

});

// Retrieve an instance of Firebase Messaging so that it can handle background
// messages.
const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function (payload) {
    ////debugger;
    //console.log('[firebase-messaging-sw.js] Received background message ', payload);
    //// Customize notification here
    //const notificationTitle = 'Medic Lab';
    //const notificationOptions = {
    //body: payload.data.message,
    //icon: '/itwonders-web-logo.png'
    //};

    ////loadMessage();

    //return self.registration.showNotification(notificationTitle,
    //    notificationOptions);
    console.log(payload);
});