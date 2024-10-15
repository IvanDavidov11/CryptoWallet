import React from 'react'
import ReloadCoinDataButton from './ReloadCoinDataButton'

const CoinTableHeader = ({ fetchCoins }) => {
  return (
    <div className="LoadedHeader">
      <h3>Your portfolio is updated every 5 minutes</h3>
      <ReloadCoinDataButton fetchCoins={fetchCoins} />
    </div>
  )
}

export default CoinTableHeader