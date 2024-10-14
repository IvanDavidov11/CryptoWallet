import React from 'react'
import Upload from './Upload'
import EmptyPortfolioHeader from './EmptyPortfolioHeader'
import BadUpload from './BadUpload'
import BadUploadHeader from './BadUploadHeader'
import { useState } from 'react'

const EmptyPortfolio = ({ onFileUpload }) => {
  const [uploadFormatFailed, setUploadFormatFailed] = useState(false);
  const [goodCoins, setGoodCoins] = useState([]);
  const [badCoins, setBadCoins] = useState([]);

  return (
    <div>
      {uploadFormatFailed ? (
        <>
          <BadUploadHeader />
          <BadUpload
            goodCoins={goodCoins}
            badCoins={badCoins}
            onFileUpload={onFileUpload}
          />
          <Upload
            uploadCaption={"Re-upload your crypto portfolio file with fixed formatting or names (.txt, .csv)"}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setBadCoins={setBadCoins}
            setGoodCoins={setGoodCoins}
          />
        </> ) : (
        <>
          <EmptyPortfolioHeader />
          <Upload
            uploadCaption={"Upload your crypto portfolio file (.txt, .csv)"}
            onFileUpload={onFileUpload}
            setUploadFormatFailed={setUploadFormatFailed}
            setBadCoins={setBadCoins}
            setGoodCoins={setGoodCoins}
          />
        </>)}

    </div>
  )
}

export default EmptyPortfolio
