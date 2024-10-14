import React from 'react'
import CoinTable from '../LoadedPortfolio/CoinPortfolio/CoinTable'
import SendGoodCoinsButton from './SendGoodCoinsButton'


const BadUpload = ({ goodCoins, badCoins, onFileUpload }) => {
  return (
    <>
      <CoinTable
        coins={badCoins}
        tableCaption={'Bad Coins (TEMP)'}
      />

      <CoinTable
        coins={goodCoins}
        tableCaption={'Good Coins (TEMP)'}
      />
      <SendGoodCoinsButton goodCoins={goodCoins} onFileUpload={onFileUpload}/>
    </>
  )
}

export default BadUpload
