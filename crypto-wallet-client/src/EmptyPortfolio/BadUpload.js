import React from 'react'
import CoinTable from '../LoadedPortfolio/CoinPortfolio/CoinTable'
import SendGoodCoinsButton from './SendGoodCoinsButton'
import SendAllCoinsForDeeperSearchButton from './SendAllCoinsForDeeperSearchButton';

const BadUpload = ({ goodCoins, setGoodCoins, badCoins, setBadCoins, onFileUpload, setUploadFormatFailed }) => {
  const hasApiCalculations = false;

  return (
    <>
      <CoinTable
        coins={badCoins}
        tableCaption={'Bad Coins (TEMP)'}
        hasApiCalculations={hasApiCalculations}
      />

      <CoinTable
        coins={goodCoins}
        tableCaption={'Good Coins (TEMP)'}
        hasApiCalculations={hasApiCalculations}
      />
      <SendGoodCoinsButton goodCoins={goodCoins} onFileUpload={onFileUpload} />
      <SendAllCoinsForDeeperSearchButton
        goodCoins={goodCoins} setGoodCoins={setGoodCoins}
        badCoins={badCoins} setBadCoins={setBadCoins}
        setUploadFormatFailed={setUploadFormatFailed}
        onFileUpload={onFileUpload}/>
    </>
  )
}

export default BadUpload