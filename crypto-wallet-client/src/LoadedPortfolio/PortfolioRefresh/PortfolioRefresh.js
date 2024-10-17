import React, { useState, useEffect } from 'react';
import RefreshPortfolioButton from './RefreshPortfolioButton';
import PortfolioRefreshSettings from './PortfolioRefreshSettings';

const PortfolioRefresh = ({ fetchCoins, fetchCurrentValue, }) => {
  const getUserPreferences_ApiUrl = 'https://localhost:7038/api/prefs'
  const [refreshInterval, setRefreshInterval] = useState(5); // in minutes

  useEffect(() => {
    const fetchRefreshInterval = async () => {
      const response = await fetch(getUserPreferences_ApiUrl);

      if (!response.ok) console.log('Fetch user data failed');
      const result = await response.json();
      setRefreshInterval(result.refreshInterval);
    }

    fetchRefreshInterval();
  }, []);

  return (
    <div>
      <div className='refreshPortfolio'>
        <p>Your portfolio is currently being updated every <strong>{refreshInterval} Minutes.</strong>
          <br />You can change this setting below:</p>
      </div>
      <div className='refreshPortfolioButtons'>
        <PortfolioRefreshSettings
          fetchCoins={fetchCoins}
          refreshInterval={refreshInterval}
          setRefreshInterval={setRefreshInterval} />
        <span>or</span>
        <RefreshPortfolioButton
          fetchCoins={fetchCoins}
          fetchCurrentValue={fetchCurrentValue}
        />
      </div>
    </div>
  )
}

export default PortfolioRefresh