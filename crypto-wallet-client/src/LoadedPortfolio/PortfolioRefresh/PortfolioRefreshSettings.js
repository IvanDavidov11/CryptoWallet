import React, { useState, useEffect } from 'react'

const PortfolioRefreshSettings = ({ fetchCoins, setRefreshInterval, refreshInterval }) => {
  const refreshOptions = [1, 2, 3, 5, 10, 15, 30, 45, 60]; // Interval values in minutes
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
    const interval = setInterval(() => {
      fetchCoins();
    }, refreshInterval * 60 * 1000) // calculates minutes into milliseconds

    return () => clearInterval(interval);
  }, [refreshInterval]);

  return (
    <>
      <select
        id="intervalDropdown"
        value={refreshInterval}
        onChange={handleIntervalChange}
        className='refreshPortfolioButton'
      >
        {refreshOptions.map((option) => (
          <option key={option} value={option}>
            {option} Minutes
          </option>
        ))}
      </select>
    </>
  );
}

export default PortfolioRefreshSettings