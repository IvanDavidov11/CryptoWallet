import React from 'react';
import RefreshPortfolioButton from './RefreshPortfolioButton';
import PortfolioRefreshSettings from './PortfolioRefreshSettings';

const PortfolioRefresh = ({fetchCoins}) => {
  return (
    <div>
      <RefreshPortfolioButton fetchCoins={fetchCoins} />
      <PortfolioRefreshSettings fetchCoins={fetchCoins} />
    </div>
  )
}

export default PortfolioRefresh
