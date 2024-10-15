import React, { useState, useEffect } from 'react'

const PortfolioRefreshSettings = ({ fetchCoins }) => {
  const [refreshInterval, setRefreshInterval] = useState(5); // in minutes
  const refreshOptions = [1, 2, 3, 5, 10, 15, 30, 45, 60]; // Interval values in minutes
  const getUserPreferences_ApiUrl = 'https://localhost:7038/api/prefs'
  const updateRefreshInterval_ApiUrl = 'https://localhost:7038/api/prefs/set-interval'

  const handleIntervalChange = async (event) => {
    const newTime = parseInt(event.target.value);
    
    try {
      const response = await fetch(updateRefreshInterval_ApiUrl, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ refreshInterval: newTime }),
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      
      setRefreshInterval(newTime);
    } catch (error) {
      console.error('Error updating interval:', error);
    }
  };

  useEffect(() => {
    const fetchRefreshInterval = async () => {
      const response = await fetch(getUserPreferences_ApiUrl);

      if (!response.ok) console.log('Fetch user data failed');
      const result = await response.json();
      setRefreshInterval(result.refreshInterval);
    }

    fetchRefreshInterval();
  }, []);

  useEffect(() => {
    const interval = setInterval(() => {
      fetchCoins();
    }, refreshInterval * 60 * 1000) // calculates minutes into milliseconds

    return () => clearInterval(interval);
  }, [refreshInterval]);

  return (
    <div>
      <h3>Your portfolio is updated every 5 minutes</h3>
      <div>
        <label htmlFor="intervalDropdown">Set Interval (minutes): </label>
        <select
          id="intervalDropdown"
          value={refreshInterval}
          onChange={handleIntervalChange}
        >
          {refreshOptions.map((option) => (
            <option key={option} value={option}>
              {option} minutes
            </option>
          ))}
        </select>
        <p>Selected interval: {refreshInterval} minutes</p>
      </div>
    </div>
  )
}

export default PortfolioRefreshSettings