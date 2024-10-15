import React, {useState, useEffect} from 'react'

const PortfolioRefreshSettings = ({ fetchCoins }) => {
  const [refreshInterval, setRefreshInterval] = useState(5); // in minutes
  const refreshOptions = [1, 2, 3, 5, 10, 15, 30, 45, 60]; // Interval values in minutes

  const handleIntervalChange = (newTime) => {
    setRefreshInterval(newTime.target.value);
  }

  useEffect(() => {
    const interval = setInterval(() =>{
      fetchCoins();
    }, refreshInterval * 60 * 1000) // calculates minutes into milliseconds

    return () => clearInterval(interval);
  })

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