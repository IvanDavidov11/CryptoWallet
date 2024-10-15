import React from 'react'

const BadUploadHeader = () => {
    return (
        <>
            <h1>Some of the coins you uploaded are badly formatted or not found in Coin Lore database</h1>
            <h3>Either re-upload the portfolio file, or just continue without the bad coins.</h3>
        </>
    )
}

export default BadUploadHeader