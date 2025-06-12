const apiUrl = "https://dummyjson.com/products/1";

// ---------- 1. Callback ----------
function fetchDataWithCallback(callback) {
  const xhr = new XMLHttpRequest();
  xhr.open("GET", apiUrl);
  xhr.onload = function () {
    if (xhr.status === 200) {
      callback(null, JSON.parse(xhr.responseText));
    } else {
      callback("Error fetching data", null);
    }
  };
  xhr.send();
}

function fetchWithCallback() {
  fetchDataWithCallback((err, data) => {
    if (err) {
      showOutput(err);
    } else {
      showOutput(`Callback: Product name is "${data.title}"`);
    }
  });
}

// ---------- 2. Promise ----------
function fetchDataWithPromise() {
  return fetch(apiUrl).then(res => {
    if (!res.ok) throw new Error("Failed to fetch");
    return res.json();
  });
}

function fetchWithPromise() {
  fetchDataWithPromise()
    .then(data => showOutput(`Promise: Product name is "${data.title}"`))
    .catch(err => showOutput(`Error: ${err.message}`));
}

// ---------- 3. Async/Await ----------
async function fetchWithAsyncAwait() {
  try {
    const response = await fetch(apiUrl);
    if (!response.ok) throw new Error("Failed to fetch");
    const data = await response.json();
    showOutput(`Async/Await: Product name is "${data.title}"`);
  } catch (error) {
    showOutput(`Error: ${error.message}`);
  }
}

// ---------- Output display ----------
function showOutput(message) {
  document.getElementById("output").textContent = message;
}
