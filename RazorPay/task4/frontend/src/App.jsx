import { useState } from 'react'
import { useEffect } from 'react';
import './App.css'

function App() {
   const [message, setMessage] = useState('');

  useEffect(() => {
    fetch('/api/message')
      .then(res => res.json())
      .then(data => setMessage(data.message));
  }, []);

   return (
    <div>
      <h1>{message || 'Loading...'}</h1>
    </div>
  );
}

export default App
